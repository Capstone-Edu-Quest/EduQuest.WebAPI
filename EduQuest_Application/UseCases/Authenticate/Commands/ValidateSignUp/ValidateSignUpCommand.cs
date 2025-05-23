﻿using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Authenticate.Commands.ValidateSignUp;

public class ValidateSignUpCommand : IRequest<APIResponse>
{
    public string Email { get; set; }
    public string Otp { get; set; }
}
