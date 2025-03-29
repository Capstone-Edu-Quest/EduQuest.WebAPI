using EduQuest_Application.Abstractions.Email;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Authenticate.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRedisCaching _redisCaching;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository,
        IRedisCaching redisCaching,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _redisCaching = redisCaching;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<APIResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        string redisDbKey = $"ResetPassword_{request.Email}";
        var user = await _userRepository.GetUserByEmailAsync(request.Email!);

        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", "user");
        }

        var otp = AuthenHelper.GenerateOTP();
        await _redisCaching.SetAsync(redisDbKey, otp, 120);

        // send otp via email (asynchronous)
        var backgroundTask = _emailService.SendEmailVerifyAsync(
            "RESET PASSWORD OTP",
            request.Email,
            user.Email,
            otp,
            "./template/VerifyWithOTP.cshtml",
            "./template/LOGO 3.png"
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.SentOtpSuccessfully, MessageCommon.SentOtpSuccessfully, "name", "user");
    }
}
