using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Authenticate.Commands.VerifyPassword;
using EduQuest_Domain.Models.Response;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

public class ValidateOtpCommandHandler : IRequestHandler<ValidateOtp, APIResponse>
{
    private readonly IRedisCaching _redisCaching;

    public ValidateOtpCommandHandler(IRedisCaching redisCaching)
    {
        _redisCaching = redisCaching;
    }

    public async Task<APIResponse> Handle(ValidateOtp request, CancellationToken cancellationToken)
    {
        var existOTP = await _redisCaching.GetAsync<string>($"ResetPassword_{request.Email}");
        if (existOTP == request.Otp)
        {
            await _redisCaching.SetAsync($"VerifiedOTP_{request.Email}", "true", 300);
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.VerifyOtp, null, "name", "user");
        }

        return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest,MessageCommon.LoginFailed, MessageCommon.LoginFailed, "otp", "user");
    }
}
