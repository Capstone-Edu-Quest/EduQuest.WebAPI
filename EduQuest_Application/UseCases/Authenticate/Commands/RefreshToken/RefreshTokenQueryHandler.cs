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

        public RefreshTokenQueryHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtProvider jwtProvider, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtProvider = jwtProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            //validate the token ưhether it's a jwt token or not, then extract information to create new token
            var principal = _jwtProvider.GetPrincipalFromExpiredToken(request.AccessToken);

            //extract userId and email from payload
            var userIdClaim = principal!.Claims.FirstOrDefault(c => c.Type == UserClaimType.UserId);
            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == UserClaimType.Email);

            //check if the user exists in the refresh token list
            var existUserToken = await _refreshTokenRepository.GetUserByIdAsync(userIdClaim!.Value.ToString());
            var existUsers = await _userRepository.GetById(userIdClaim!.Value);
            if (existUserToken == null)
            {
                return new APIResponse
                {
                    IsError = true,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.Unauthorized,
                        Message = "Invalid token"
                    },
                    Payload = null
                };
            }
            else if (existUsers!.Status == AccountStatus.Blocked.ToString())
            {
                return new APIResponse
                {
                    IsError = true,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.Unauthorized,
                        Message = "HARD CODE"
                    },
                    Payload = null
                };
            }


            //check refresh token whether it's expired or null
            var existingRefreshToken = await _refreshTokenRepository.GetTokenAsync(request.RefreshToken!);
            if (existingRefreshToken == null || existingRefreshToken.ExpireAt <= DateTime.UtcNow)
            {
                if (existingRefreshToken != null && existingRefreshToken.ExpireAt <= DateTime.UtcNow)
                {
                    await _refreshTokenRepository.Delete(existingRefreshToken.Id);
                }
                return new APIResponse
                {
                    IsError = true,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.Unauthorized,
                        Message = "Token expired"
                    },
                    Payload = null
                };
            }


            //capture expired date from original token 
            var originalExpirationDate = existingRefreshToken.ExpireAt;

            // remove old refresh token
            await _refreshTokenRepository.Delete(existingRefreshToken.Id);

            // generate new tokens
            var tokenResponse = await _jwtProvider.GenerateAccessRefreshTokens(existUsers.Id, existUsers.Email!);

            await _unitOfWork.SaveChangesAsync();

            return new APIResponse
            {
                IsError = true,
                Errors = null,
                Payload = new TokenResponseDTO
                {
                    AccessToken = tokenResponse.AccessToken,
                    RefreshToken = tokenResponse.RefreshToken
                }
            };
        }
    }
}
