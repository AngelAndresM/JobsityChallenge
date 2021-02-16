using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using JobsityChat.Core.Models;
using JobsityChat.Core.Contracts;
using JobsityChat.WebApi.Models;

namespace JobsityChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly UserManager<UserInfo> _userManager;
        private readonly ITokenService _tokenService;

        public AuthenticationController(ILogger<AuthenticationController> logger, SignInManager<UserInfo> signInManager,
                                        ITokenService tokenService, UserManager<UserInfo> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = string.Empty,
                    HasError = true,
                    ValidationMessage = "Login attempt failed!, all fields are required."
                });
            }

            var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                return Unauthorized(new
                {
                    Message = string.Empty,
                    HasError = true,
                    ValidationMessage = "Invalid login attempt!, please check your user or password. "
                });
            }

            var tokenString = _tokenService.GetNewToken(model.UserName);
            var user = await _userManager.FindByNameAsync(model.UserName);

            return Ok(new
            {
                Message = "Login success!",
                HasError = false,
                ValidationMessage = string.Empty,
                Token = tokenString,
                userDetail = new
                {
                    UserName = user.UserName,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Email = user.Email
                }
            });
        }
    }
}
