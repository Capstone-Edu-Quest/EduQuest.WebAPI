using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Authenticate.Commands.VerifyPassword;

public class ValidateOtp : IRequest<APIResponse>
{
    public string? Email { get; set; }
    public string? Otp { get; set; }

    public ValidateOtp(string? email, string? otp)
    {
        Email = email;
        Otp = otp;
    }
}
