using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Authenticate.Commands.RefreshToken
{
    public class RefreshTokenQuery : IRequest<APIResponse>
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public RefreshTokenQuery(string? accessToken, string? refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
