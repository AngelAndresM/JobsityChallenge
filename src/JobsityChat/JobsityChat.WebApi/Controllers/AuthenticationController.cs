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
        private readonly ITokenService _tokenService;

        public AuthenticationController(ILogger<AuthenticationController> logger, SignInManager<UserInfo> signInManager,
                                        ITokenService tokenService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _tokenService = tokenService;
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

            return Ok(new { Message = "Login success!", HasError = false, Token = tokenString });
        }
    }
}
