using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using JobsityChat.Core.Models;
using JobsityChat.WebApi.Models;

namespace JobsityChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<UserInfo> _manager;

        public UserController(ILogger<UserController> logger, UserManager<UserInfo> userManager)
        {
            _logger = logger;
            _manager = userManager;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = string.Empty, HasError = true, ValidationMessage = "Registration failed!, all fields are required." });
            }

            var userRecord = new UserInfo()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _manager.CreateAsync(userRecord, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = string.Empty, HasError = true, ValidationMessage = "Registration failed!", ValidationError = result.Errors });
            }

            return Ok(new { Message = "Successful registration!", HasError = false, ValidationMessage = string.Empty });
        }
    }
}
