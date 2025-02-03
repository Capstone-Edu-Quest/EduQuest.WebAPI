using EduQuest_Application.Abstractions.Oauth2;
using EduQuest_Domain.Models.Oauth2;
using EduQuest_Domain.Models.Response;
using EduQuest_Infrastructure.ExternalServices.Oauth2.Setting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace Infrastructure.ExternalServices.Oauth2;

public class TokenValidation : ITokenValidation
{
    private readonly GoogleSetting _setting;

    public TokenValidation(IOptions<GoogleSetting> setting)
    {
        _setting = setting.Value;
    }

    public async Task<APIResponse> ValidateGoogleTokenAsync(string token)
    {

        //using (var httpClient = new HttpClient())
        //{
        //    var response = await httpClient.GetAsync($"{_setting.Url}{token}");
        //    if (response.StatusCode != HttpStatusCode.OK)
        //    {
        //        return new APIResponse
        //        {
        //           IsError = true,
        //           Payload = null,
        //           Errors = new ErrorResponse
        //           {
        //               StatusCode = (int)response.StatusCode,
        //               Message = MessageCommon.NotFound
        //           }
        //        };
        //    }

        //    var responseString = await response.Content.ReadAsStringAsync();
        //    var tokenInfo = JsonConvert.DeserializeObject<GoogleTokenInfo>(responseString);

        //    return new APIResponse
        //    {
        //        IsError = true,
        //        Payload = tokenInfo,
        //        Errors = null
        //    };
        //}
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jsonToken == null)
        {
            return new APIResponse
            {
                IsError = true,
                Payload = null,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid token"
                }
            };
        }

        var expClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "exp");
        var expDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value)).DateTime;
        if (expDateTime < DateTime.UtcNow)
        {
            return new APIResponse
            {
                IsError = true,
                Payload = null,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Token has expired"
                }
            };
        }

        // Lấy thông tin từ payload
        var payload = jsonToken.Payload;

        var tokenInfo = new GoogleTokenInfo
        {
            FullName = payload.ContainsKey("name") ? payload["name"].ToString() : string.Empty,
            Email = payload.ContainsKey("email") ? payload["email"].ToString() : string.Empty,
            picture = payload.ContainsKey("picture") ? payload["picture"].ToString() : string.Empty
        };

        return new APIResponse
        {
            IsError = false,
            Payload = tokenInfo,
            Errors = null
        };
    }
}

