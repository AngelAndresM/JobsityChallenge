using System;
using Newtonsoft.Json;

namespace JobsityChat.WebUI.Models.Response
{
    public class LoginResponseModel
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("userDetail")]
        public UserDetailResponseModel UserDetail { get; set; }

        [JsonProperty("hasError")]
        public bool HasError { get; set; }

        [JsonProperty("validationMessage")]
        public string ValidationMessage { get; set; }
    }

    public class UserDetailResponseModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
