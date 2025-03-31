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

    public ValidateOtpCommandHandler(IRedisCaching redisCaching, IEmailService emailService, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _redisCaching = redisCaching;
        _emailService = emailService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(ValidateOtp request, CancellationToken cancellationToken)
    {
        var existOTP = await _redisCaching.GetAsync<string>($"ResetPassword_{request.Email}");
        if (existOTP != request.Otp)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.LoginFailed, MessageCommon.LoginFailed, "otp", "user");
        }

        if (request.isChangePassword)
        {
            await _redisCaching.SetAsync($"VerifiedOTP_{request.Email}", "true", 300);
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.VerifyOtp, null, "name", "user");
        }

        var user = await _userRepository.GetUserByEmailAsync(request.Email!);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.LoginFailed, MessageCommon.LoginFailed, "otp", "user");
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
        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.ResetPasswordSuccessfully, null, "name", "user");

    }
}
