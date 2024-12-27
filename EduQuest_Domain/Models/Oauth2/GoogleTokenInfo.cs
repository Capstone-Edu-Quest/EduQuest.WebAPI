using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Models.Oauth2
{
    public class GoogleTokenInfo
    {
        [JsonProperty("azp")]
        public string? AuthorizedParty { get; set; }

        [JsonProperty("aud")]
        public string? Audience { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }
    }
}
