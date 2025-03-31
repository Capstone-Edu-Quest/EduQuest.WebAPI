using EduQuest_Application.Abstractions.Email;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Authenticate.Commands.ValidateChangePassword;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

public class ValidateOtpCommandHandler : IRequestHandler<ValidateOtp, APIResponse>
{
    private readonly IRedisCaching _redisCaching;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public ValidateOtpCommandHandler(IRedisCaching redisCaching, IEmailService emailService, IUserRepository userRepository, IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository)
    {
        _redisCaching = redisCaching;
        _emailService = emailService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<APIResponse> Handle(ValidateOtp request, CancellationToken cancellationToken)
    {
        var existOTP = await _redisCaching.GetAsync<string>($"ResetPassword_{request.Email}");
        if (existOTP != request.Otp)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.VerifyOtpFailed, MessageCommon.VerifyOtpFailed, "name", "otp");
        }

        var user = await _userRepository.GetUserByEmailAsync(request.Email!);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.EmailNotFound, MessageCommon.EmailNotFound, "otp", request.Email ?? "");
        }

        if (request.isChangePassword)
        {

            var oldPassword = await _redisCaching.HashGetSpecificKeyAsync($"ChangePassword_{user.Email}", "oldpassword");
            var newpassword = await _redisCaching.HashGetSpecificKeyAsync($"ChangePassword_{user.Email}", "newpassword");
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newpassword))
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.LoginFailed, MessageCommon.LoginFailed, "name", "user");
            }

            if (!AuthenHelper.VerifyPasswordHash(oldPassword, user.PasswordHash!, user.PasswordSalt!))
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.WrongPassword, MessageCommon.WrongPassword, "name", "password");
            }

            AuthenHelper.CreatePasswordHash(newpassword, out byte[] hash, out byte[] salt);
            user.PasswordHash = Convert.ToBase64String(hash);
            user.PasswordSalt = Convert.ToBase64String(salt);

            var existingTokens = await _refreshTokenRepository.GetRefreshTokenByUserId(user.Id);
            if (existingTokens != null)
            {
                foreach (var token in existingTokens)
                {
                    await _refreshTokenRepository.RemoveRefreshTokenAsync(token.Id);
                }
            }

            await _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _redisCaching.RemoveAsync($"ChangePassword_{user.Email}");
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.PasswordChanged, null, "name", user.Email ?? "");
        }

  

        var newPassword = AuthenHelper.GenerateRandomPassword();
        AuthenHelper.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
        var backgroundTask = _emailService.SendEmailVerifyAsync(
                                    "YOUR NEW PASSWORD ON EDUQUEST",
                                    user.Email,
                                    "",
                                    newPassword,
                                    "./template/VerifyWithOTP.cshtml",
                                    "./template/LOGO 3.png"
                                );
        user.PasswordHash = Convert.ToBase64String(passwordHash);
        user.PasswordSalt = Convert.ToBase64String(passwordSalt);

        await _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _redisCaching.RemoveAsync($"ResetPassword_{user.Email}");
        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.ResetPasswordSuccessfully, null, "name", user.Email ?? "");

    }
}
