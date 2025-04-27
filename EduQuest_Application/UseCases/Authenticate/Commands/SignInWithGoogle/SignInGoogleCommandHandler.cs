using AutoMapper;
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Email;
using EduQuest_Application.Abstractions.Oauth2;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.ExternalServices.QuartzService;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Oauth2;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Authenticate.Commands.SignInWithGoogle
{
    public class SignInGoogleCommandHandler : IRequestHandler<SignInGoogleCommand, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenValidation _googleTokenValidation;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;
        private readonly IEmailService _emailService;
        private readonly IQuartzService _quartzService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SignInGoogleCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ITokenValidation googleTokenValidation,
            IMapper mapper,
            IJwtProvider jwtProvider,
            IEmailService emailService,
            IQuartzService quartzService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _googleTokenValidation = googleTokenValidation;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _emailService = emailService;
            _quartzService = quartzService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<APIResponse> Handle(SignInGoogleCommand request, CancellationToken cancellationToken)
        {
            var tokenValidationResponse = await _googleTokenValidation.ValidateGoogleTokenAsync(request.Token!);
            if (tokenValidationResponse.IsError)
            {
                return tokenValidationResponse;
            }

            var tokenInfo = tokenValidationResponse.Payload as GoogleTokenInfo;
            var user = await _userRepository.GetUserByEmailAsync(tokenInfo!.Email!);

            if (user == null)
            {
                user = await CreateNewUserAsync(tokenInfo);
                await SendWelcomeEmailAsync(user.Email!, user.PasswordHash);
                await _quartzService.AddAllQuestsToNewUser(user.Id);
            }

            var tokens = await _jwtProvider.GenerateTokensForUser(user.Id, user.Email!, Guid.NewGuid().ToString());
            await _unitOfWork.SaveChangesAsync();
            SetAuthCookies(tokens.AccessToken, tokens.RefreshToken);

            var userDto = _mapper.Map<UserResponseDto>(user);

            return new APIResponse
            {
                IsError = false,
                Errors = null,
                Payload = new LoginResponseDto
                {
                    UserData = userDto,
                    Token = tokens
                }
            };
        }

        private async Task<User> CreateNewUserAsync(GoogleTokenInfo tokenInfo)
        {
            var userId = Guid.NewGuid().ToString();
            var newPassword = AuthenHelper.GenerateRandomPassword();
            AuthenHelper.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                Id = userId,
                Email = tokenInfo.Email,
                Username = tokenInfo.FullName,
                AvatarUrl = tokenInfo.picture,
                Status = AccountStatus.Active.ToString(),
                RoleId = ((int)GeneralEnums.UserRole.Learner).ToString(),
                Package = GeneralEnums.PackageEnum.Free.ToString(),
                PasswordHash = Convert.ToBase64String(passwordHash),
                PasswordSalt = Convert.ToBase64String(passwordSalt),
                UserMeta = new UserMeta
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    LastActive = DateTime.UtcNow,
                    CurrentStreak = 0,
                    LongestStreak = 0,
                    TotalCompletedCourses = 0,
                    Gold = 0,
                    Exp = 0,
                    Level = 1,
                    TotalStudyTime = 0,
                    TotalCourseCreated = 0,
                    TotalLearner = 0,
                    TotalReview = 0
                },
                FavoriteList = new FavoriteList
                {
                    UserId = userId
                }
            };

            await _userRepository.Add(newUser);
            await _unitOfWork.SaveChangesAsync();
            return newUser;
        }

        private async Task SendWelcomeEmailAsync(string email, string password)
        {
            // Gửi email xác nhận trong background task để không block yêu cầu.
            var backgroundTask = _emailService.SendEmailVerifyAsync(
                    "YOUR PASSWORD ON EDUQUEST",
                    email,
                    "",
                    password,
                    "./template/VerifyWithOTP.cshtml",
                    "./template/LOGO 3.png"
                ); // Chờ email được gửi xong trong background
        }

        private void SetAuthCookies(string accessToken, string refreshToken)
        {
            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMonths(3)
            };

            _httpContextAccessor.HttpContext!.Response.Cookies.Append("access_token", accessToken, accessCookieOptions);
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("refresh_token", refreshToken, refreshCookieOptions);
        }
    }
}
