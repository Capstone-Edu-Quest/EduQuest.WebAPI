using AutoMapper;
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Email;
using EduQuest_Application.Abstractions.Oauth2;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.ExternalServices.QuartzService;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Oauth2;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;

namespace Application.UseCases.Authenticate.Commands.SignInWithGoogle
{
    public class SignInGoogleCommandHandler : IRequestHandler<SignInGoogleCommand, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenValidation _googleTokenValidation;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;
        private readonly IEmailService _emailService;
        private readonly IQuartzService _quartzService;

        public SignInGoogleCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, 
            IUnitOfWork unitOfWork, ITokenValidation googleTokenValidation, IMapper mapper, 
            IJwtProvider jwtProvider, IEmailService emailService, IQuartzService quartzService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _googleTokenValidation = googleTokenValidation;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _emailService = emailService;
            _quartzService = quartzService;
        }

        public async Task<APIResponse> Handle(SignInGoogleCommand request, CancellationToken cancellationToken)
        {
            var tokenValidationResponse = await _googleTokenValidation.ValidateGoogleTokenAsync(request.Token!);
            if (tokenValidationResponse.IsError)
            {
                return tokenValidationResponse;
            }

            var tokenInfo = tokenValidationResponse.Payload as GoogleTokenInfo;
            var newPassword = AuthenHelper.GenerateRandomPassword();
            AuthenHelper.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            var user = await _userRepository.GetUserByEmailAsync(tokenInfo!.Email!);
            if (user == null)
            {
                var userId = Guid.NewGuid().ToString();
                var newUser = new User
                {
                    Id = userId,
                    Email = tokenInfo!.Email,
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
                        CurrentStreak = 0,
                        LongestStreak = 0,
                        TotalCompletedCourses = 0,
                        Gold = 0,
                        Exp = 0,
                        Level = 0,
                        TotalStudyTime = 0,
                        TotalCourseCreated = 0,
                        TotalLearner = 0,
                        TotalReview = 0,
                        LastActive = DateTime.UtcNow.ToUniversalTime(),
                        CreatedAt = DateTime.Now.ToUniversalTime(),
                        UpdatedAt = DateTime.Now.ToUniversalTime(),
                    },
                    FavoriteList = new FavoriteList
                    {
                        UserId = userId
                    }
                };

                await _userRepository.Add(newUser);

                await _unitOfWork.SaveChangesAsync();

                var response = await _jwtProvider.GenerateTokensForUser(newUser.Id, newUser.Email!, Guid.NewGuid().ToString());

                await _unitOfWork.SaveChangesAsync();

                var backgroundTask = _emailService.SendEmailVerifyAsync(
                                    "YOUR PASSWORD ON EDUQUEST",
                                    newUser.Email!,
                                    "",
                                    newPassword,
                                    "./template/VerifyWithOTP.cshtml",
                                    "./template/LOGO 3.png"
                                );
                await _quartzService.AddAllQuestsToNewUser(userId);
                var data = _mapper.Map<UserResponseDto>(newUser);
                return new APIResponse
                {
                    IsError = false,
                    Errors = null,
                    Payload = new LoginResponseDto
                    {
                        UserData = data,
                        Token = response
                    }
                };

            };


            //create new refresh token
            var tokenResponse = await _jwtProvider.GenerateTokensForUser(user.Id, user.Email!, Guid.NewGuid().ToString());
            await _unitOfWork.SaveChangesAsync();

            var userDTO = _mapper.Map<UserResponseDto>(user);

            return new APIResponse
            {
                IsError = false,
                Errors = null,
                Payload = new LoginResponseDto
                {
                    UserData = userDTO,
                    Token = tokenResponse
                }
            };
        }
    }
}
