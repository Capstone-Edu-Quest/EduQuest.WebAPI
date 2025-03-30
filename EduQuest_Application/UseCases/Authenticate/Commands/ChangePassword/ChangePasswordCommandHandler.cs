using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Authenticate.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCaching _redisCaching;

    public ChangePasswordCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IRedisCaching redisCaching)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _redisCaching = redisCaching;
    }

    public async Task<APIResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(request.UserId);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", "user");
        }

        var verifiedOTP = await _redisCaching.GetAsync<string>($"VerifiedOTP_{user.Email}");
        if (verifiedOTP != "true")
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.Forbidden, "OTP validation required", "OTP validation required", "name", "user");
        }

        if (!AuthenHelper.VerifyPasswordHash(request.OldPassword!, user.PasswordHash!, user.PasswordSalt!))
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, "Old password is incorrect", "Old password is incorrect", "password", "user");
        }

        AuthenHelper.CreatePasswordHash(request.NewPassword!, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = Convert.ToBase64String(passwordHash);
        user.PasswordSalt = Convert.ToBase64String(passwordSalt);

        var existingTokens = await _refreshTokenRepository.GetRefreshTokenByUserId(user.Id);
        if (existingTokens != null)
        {
            foreach (var token in existingTokens)
            {
                await _refreshTokenRepository.RemoveRefreshTokenAsync(token.Id);
            }
        }

        await _redisCaching.RemoveAsync($"VerifiedOTP_{user.Email}");
        await _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, "Password changed successfully", "Password changed successfully", "password", "user");
    }
}
