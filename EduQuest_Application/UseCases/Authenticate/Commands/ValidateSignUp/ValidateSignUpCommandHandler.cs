using AutoMapper;
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Application.Helper;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.ExternalServices.QuartzService;
using Microsoft.AspNetCore.Http;

namespace EduQuest_Application.UseCases.Authenticate.Commands.ValidateSignUp;

public class ValidateSignUpCommandHandler : IRequestHandler<ValidateSignUpCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRedisCaching _redisCaching;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IQuartzService _quartzService;

    public ValidateSignUpCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtProvider jwtProvider, IRedisCaching redisCaching, IHttpContextAccessor httpContextAccessor, IMapper mapper, IQuartzService quartzService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
        _redisCaching = redisCaching;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _quartzService = quartzService;
    }

    public async Task<APIResponse> Handle(ValidateSignUpCommand request, CancellationToken cancellationToken)
    {
        var redisKey = $"SignUp_{request.Email}";
        var redisData = await _redisCaching.GetAllHashDataAsync(redisKey);
        if (redisData == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.VerifyOtpFailed, MessageCommon.VerifyOtpFailed, "otp", "");
        }

        var dict = redisData.ToDictionary(entry => entry.Key.ToString(), entry => entry.Value.ToString());
        if (!dict.TryGetValue("otp", out string storedOtp) || storedOtp != request.Otp)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.VerifyOtpFailed, MessageCommon.VerifyOtpFailed, "otp", request.Otp);
        }

        var userId = Guid.NewGuid().ToString();
        var newUser = new User
        {
            Id = userId,
            Email = dict["email"],
            Username = dict["fullname"],
            AvatarUrl = null,
            Status = AccountStatus.Active.ToString(),
            RoleId = ((int)GeneralEnums.UserRole.Learner).ToString(),
            Package = GeneralEnums.PackageEnum.Free.ToString(),
            PasswordHash = dict["passwordHash"],
            PasswordSalt = dict["passwordSalt"],
            UserMeta = new UserMeta
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                CurrentStreak = 0,
                LongestStreak = 0,
                TotalCompletedCourses = 0,
                Gold = 0,
                Exp = 0,
                Level = 1,
                TotalStudyTime = 0,
                TotalCourseCreated = 0,
                TotalLearner = 0,
                TotalReview = 0,
                LastActive = DateTime.UtcNow.ToUniversalTime(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            FavoriteList = new FavoriteList
            {
                UserId = userId
            }
        };

        await _userRepository.Add(newUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var tokenResponse = await _jwtProvider.GenerateTokensForUser(newUser.Id, newUser.Email, Guid.NewGuid().ToString());

        var accessToken = tokenResponse.AccessToken;
        var refreshToken = tokenResponse.RefreshToken;

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, 
            Secure = true,
            SameSite = SameSiteMode.Strict, 
            Expires = DateTime.UtcNow.AddMinutes(15) 
        };

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMonths(3) 
        };

        _httpContextAccessor.HttpContext!.Response.Cookies.Append("access_token", accessToken, cookieOptions);
        _httpContextAccessor.HttpContext!.Response.Cookies.Append("refresh_token", refreshToken, refreshCookieOptions);
        var userDTO = _mapper.Map<UserResponseDto>(newUser);

        await _quartzService.AddAllQuestsToNewUser(userId);
        await _redisCaching.RemoveAsync(redisKey);

        return new APIResponse
        {
            IsError = false,
            Payload = new LoginResponseDto
            {
                UserData = userDTO,
                Token = tokenResponse
            }
        };
    }
}
