using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Authenticate.Commands.RefreshToken
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenQueryHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtProvider jwtProvider,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtProvider = jwtProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            // Validate JWT token
            var principal = _jwtProvider.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return new APIResponse
                {
                    IsError = true,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.Unauthorized,
                        Message = MessageCommon.InvalidToken
                    },
                    Payload = null,
                    Message = new MessageResponse { content = MessageCommon.InvalidToken }
                };
            }

            // Extract user ID from token claims
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == UserClaimType.UserId);
            if (userIdClaim == null)
            {
                return new APIResponse
                {
                    IsError = true,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.Unauthorized,
                        Message = MessageCommon.InvalidToken
                    },
                    Payload = null,
                    Message = new MessageResponse { content = MessageCommon.InvalidToken }
                };
            }

            var userId = userIdClaim.Value;

            // Check if user exists in token repository
            var existUserToken = await _refreshTokenRepository.GetUserByIdAsync(userId);
            if (existUserToken == null)
            {
                return new APIResponse
                {
                    IsError = true,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.NotFound,
                        Message = MessageCommon.NotFound
                    },
                    Payload = null,
                    Message = new MessageResponse { content = MessageCommon.NotFound }
                };
            }

            var existUser = await _userRepository.GetById(userId);
            if (existUser == null || existUser.Status == AccountStatus.Blocked.ToString())
            {
                return new APIResponse
                {
                    IsError = true,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.Unauthorized,
                        Message = MessageCommon.Blocked
                    },
                    Payload = null,
                    Message = new MessageResponse { content = MessageCommon.Blocked }
                };
            }

            // Validate refresh token
            if (existUserToken.ExpireAt <= DateTime.UtcNow)
            {
                await _refreshTokenRepository.Delete(existUserToken.Id);
                return new APIResponse
                {
                    IsError = true,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.Unauthorized,
                        Message = MessageCommon.TokenExpired
                    },
                    Payload = null,
                    Message = new MessageResponse { content = MessageCommon.TokenExpired }
                };
            }

            // Remove old refresh token
            await _refreshTokenRepository.Delete(existUserToken.Id);

            // Generate new access & refresh tokens
            var tokenResponse = await _jwtProvider.GenerateAccessRefreshTokens(existUser.Id, existUser.Email!);

            existUser.LastActiveDay = DateTime.UtcNow.ToUniversalTime();
            await _userRepository.Update(existUser);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new APIResponse
            {
                IsError = false,
                Errors = null,
                Payload = new TokenResponseDTO
                {
                    AccessToken = tokenResponse.AccessToken,
                    RefreshToken = tokenResponse.RefreshToken
                },
                Message = new MessageResponse { content = MessageCommon.TokenRefreshSuccess }
            };
        }
    }
}
