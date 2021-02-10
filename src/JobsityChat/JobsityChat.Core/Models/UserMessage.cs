using System;

namespace JobsityChat.Core.Models
{
    public partial class UserMessage
    {
        public System.Guid MessageId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
