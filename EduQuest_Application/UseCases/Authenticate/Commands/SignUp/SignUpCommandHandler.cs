using AutoMapper;
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Email;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using EduQuest_Application.Helper;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using StackExchange.Redis;

namespace EduQuest_Application.UseCases.Authenticate.Commands.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCaching _redisCaching;
    private readonly IEmailService _emailService;

    public SignUpCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IRedisCaching redisCaching, IEmailService emailService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _redisCaching = redisCaching;
        _emailService = emailService;
    }

    public async Task<APIResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.ConfirmPassword))
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.LoginFailed, MessageCommon.LoginFailed, "name", "");
        }

        if (request.Password != request.ConfirmPassword)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.WrongPassword, MessageCommon.WrongPassword, "name", "password");
        }

        var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.Conflict, MessageCommon.EmailExisted, MessageCommon.EmailExisted, "name", request.Email ?? "");
        }

        AuthenHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var otp = AuthenHelper.GenerateOTP();

        // send OTP via email
        var sendTask = _emailService.SendEmailVerifyAsync(
            "SIGN UP VERIFICATION OTP",
            request.Email,
            "",
            otp,
            "./VerifyWithOTP.cshtml",
            "./LOGO 3.png"
        );

        var hashEntries = new[]
        {
            new HashEntry("otp", otp),
            new HashEntry("fullname", request.FullName),
            new HashEntry("email", request.Email),
            new HashEntry("passwordHash", Convert.ToBase64String(passwordHash)),
            new HashEntry("passwordSalt", Convert.ToBase64String(passwordSalt))
        };

        await _redisCaching.HashSetAsync($"SignUp_{request.Email}", hashEntries, 300);

        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.SentOtpSuccessfully, null, "name", "user");
    }
}
