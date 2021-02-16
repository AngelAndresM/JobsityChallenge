using System;
using Newtonsoft.Json;

namespace JobsityChat.WebUI.Models.Response
{
    public class ChatMessageResponseModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("userFullName")]
        public string UserFullName { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
    }
}
