using System;

namespace JobsityChat.WebApi.Models
{
    public class ChatMessageViewModel
    {
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string Message { get; set; }
        public string CreatedAt { get; set; }
    }
}
