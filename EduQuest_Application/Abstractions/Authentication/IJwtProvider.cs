using EduQuest_Application.DTO.Response;
using System.Security.Claims;

namespace EduQuest_Application.Abstractions.Authentication
{
	public interface IJwtProvider
    {
        /// <summary>
        /// generate access token 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<string> GenerateAccessToken(string email);

        /// <summary>
        /// validate old token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);

        /// <summary>
        /// Generate both access token and refresh token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<TokenResponseDTO> GenerateAccessRefreshTokens(string userId, string email);
    }
}
