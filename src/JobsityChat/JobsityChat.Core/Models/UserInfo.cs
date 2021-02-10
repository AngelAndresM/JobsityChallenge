using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace JobsityChat.Core.Models
{
    public class UserInfo : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<UserMessage> Messages { get; set; }
    }
}
