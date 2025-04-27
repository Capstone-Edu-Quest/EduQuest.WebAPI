using AutoMapper;
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Authenticate.Commands.SignInWithPassword
{
    public class SignInWithPasswordHandler : IRequestHandler<SignInWithPassword, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;

        public SignInWithPasswordHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IJwtProvider jwtProvider, IMapper mapper)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(SignInWithPassword request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.EmailNotFound, MessageCommon.EmailNotFound, "name", request.Email ?? "");
            }

            if (!AuthenHelper.VerifyPasswordHash(request.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, Constants.MessageCommon.WrongPassword, MessageCommon.WrongPassword, "name", "password");
            }


            //var existingRefreshToken = await _refreshTokenRepository.GetByUserIdAndDevice(deviceId);
            var tokens = await _jwtProvider.GenerateTokensForUser(user.Id, user.Email!, Guid.NewGuid().ToString());

            var accessToken = tokens.AccessToken;
            var refreshToken = tokens.RefreshToken;

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
            await _unitOfWork.SaveChangesAsync();

            var userDTO = _mapper.Map<UserResponseDto>(user);
            return new APIResponse
            {
                IsError = false,
                Errors = null,
                Payload = new LoginResponseDto
                {
                    UserData = userDTO,
                    Token = new TokenResponseDTO
                    {
                        AccessToken = tokens.AccessToken,
                        RefreshToken = tokens.RefreshToken,
                    }
                }
            };
        }

    }
}
