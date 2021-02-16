using System;
using Newtonsoft.Json;

namespace JobsityChat.WebUI.Models.Response
{
    public class RegisterResponseModel
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("hasError")]
        public bool HasError { get; set; }

        [JsonProperty("validationMessage")]
        public string ValidationMessage { get; set; }
    }
}
