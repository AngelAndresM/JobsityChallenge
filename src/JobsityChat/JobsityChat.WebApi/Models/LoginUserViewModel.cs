using System;
using System.ComponentModel.DataAnnotations;

namespace JobsityChat.WebApi.Models
{
    public class LoginUserViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
