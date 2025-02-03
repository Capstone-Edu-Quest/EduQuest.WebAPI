using EduQuest_Domain.Models.Response;

namespace EduQuest_Application.Abstractions.Oauth2
{
    public interface ITokenValidation
	{
		Task<APIResponse> ValidateGoogleTokenAsync(string token);
	}
}
