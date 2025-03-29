using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
namespace EduQuest_Application.UseCases.Authenticate.Commands.RefreshToken
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserMetaRepository _userStatisticRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;
        public RefreshTokenQueryHandler(IUserRepository userRepository, IUserMetaRepository userStatisticRepository, IRefreshTokenRepository refreshTokenRepository, IJwtProvider jwtProvider, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userStatisticRepository = userStatisticRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtProvider = jwtProvider;
            _unitOfWork = unitOfWork;
        }
        public async Task<APIResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var principal = _jwtProvider.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.InvalidToken, MessageCommon.InvalidToken, "name", "token");
            }
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == UserClaimType.UserId);
            var userEmailClaim = principal.Claims.FirstOrDefault(c => c.Type == UserClaimType.Email);
            if (userIdClaim == null || userEmailClaim == null)
            {
                return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.InvalidToken, MessageCommon.InvalidToken, "name", "token");
            }
            var userId = userIdClaim.Value;
            var email = userEmailClaim.Value;

            //extract token Id from refresh token
            var tokenId = AuthenHelper.ExtractIdFromRefreshToken(request.RefreshToken);
            if (string.IsNullOrEmpty(tokenId))
            {
                return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.InvalidToken, MessageCommon.InvalidToken, "name", "token");
            }

            //check if refresh token expired or not
            var existUserToken = await _refreshTokenRepository.GetById(tokenId);
            if (existUserToken == null || existUserToken.ExpireAt <= DateTime.UtcNow)
            {
                if (existUserToken != null)
                {
                    await _refreshTokenRepository.Delete(existUserToken);
                }
                return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.TokenExpired, MessageCommon.TokenExpired, "name", "token");
            }

            //generate new access token, refresh token with rotation
            var tokens = await _jwtProvider.GenerateTokensForUser(userId, email, existUserToken.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new APIResponse
            {
                IsError = false,
                Payload = new TokenResponseDTO
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken
                }
            };
        }
    }
}