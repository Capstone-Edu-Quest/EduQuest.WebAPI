using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Authenticate.Commands.ValidateChangePassword;

public class ValidateOtp : IRequest<APIResponse>
{
    public string? Email { get; set; }
    public string? Otp { get; set; }
    public bool isChangePassword { get; set; }
}
