using Newtonsoft.Json;

namespace EduQuest_Domain.Models.Oauth2;

public class GoogleTokenInfo
{
    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("name")]
    public string? FullName { get; set; }

    [JsonProperty("picture")]
    public string? picture { get; set; }
}
