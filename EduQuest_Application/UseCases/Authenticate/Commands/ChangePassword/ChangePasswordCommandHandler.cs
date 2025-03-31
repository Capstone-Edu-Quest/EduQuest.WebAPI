using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Email;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using StackExchange.Redis;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Authenticate.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCaching _redisCaching;
    private readonly IEmailService _emailService;

    public ChangePasswordCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IRedisCaching redisCaching, IEmailService emailService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _redisCaching = redisCaching;
        _emailService = emailService;
    }

    public async Task<APIResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.EmailNotFound, MessageCommon.EmailNotFound, "name", request.Email ?? "");
        }

        string redisDbKey = $"ResetPassword_{user.Email}";
        var otp = AuthenHelper.GenerateOTP();
        await _redisCaching.SetAsync(redisDbKey, otp, 30);

        // send otp via email (asynchronous)
        var a = _emailService.SendEmailVerifyAsync(
            "RESET PASSWORD OTP",
            user.Email,
            "",
            otp,
            "./VerifyWithOTP.cshtml",
            "./LOGO 3.png"
        );

        var hashEntries = new[]
                        {
                            new HashEntry("oldpassword", request.OldPassword),
                            new HashEntry("newpassword", request.NewPassword)
                        };

        await _redisCaching.HashSetAsync($"ChangePassword_{user.Email}", hashEntries, 30);

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.SentOtpSuccessfully, null, "name", "user");
    }
}
