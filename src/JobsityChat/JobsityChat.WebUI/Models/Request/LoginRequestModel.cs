using System;
using Newtonsoft.Json;

namespace JobsityChat.WebUI.Models.Request
{
    public class LoginRequestModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
