using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Newtonsoft.Json;

using JobsityChat.WebUI.Models;
using JobsityChat.WebUI.Services;
using System.Security.Claims;

namespace JobsityChat.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IJobsityApi _jobsityApi;

        public AccountController(IJobsityApi jobsityApi)
        {
            _jobsityApi = jobsityApi;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var request = new Models.Request.LoginRequestModel
                {
                    UserName = user.UserName,
                    Password = user.Password
                };
                var response = await _jobsityApi.LoginAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<Models.Response.LoginResponseModel>(responseString);

                if (response.IsSuccessStatusCode && !responseObj.HasError)
                {
                    var claims = new ClaimsIdentity("Bearer");
                    claims.AddClaim(new Claim(ClaimTypes.Authentication, responseObj.Token));
                    claims.AddClaim(new Claim(ClaimTypes.Name, responseObj.UserDetail.UserName));
                    claims.AddClaim(new Claim(ClaimTypes.Email, responseObj.UserDetail.Email));
                    claims.AddClaim(new Claim(ClaimTypes.GivenName, responseObj.UserDetail.FullName));
                    claims.AddClaim(new Claim(ClaimTypes.Expiration, TimeSpan.FromDays(7).ToString()));

                    var principal = new ClaimsPrincipal(claims);

                    await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("*", "Invalid login attempt");
                }
            }

            return View(user);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = new Models.Request.RegisterRequestModel
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.UserName,
                    Password = model.Password
                };

                var response = await _jobsityApi.RegisterAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<Models.Response.RegisterResponseModel>(responseString);

                if (response.IsSuccessStatusCode && !responseObj.HasError)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("*", "Oops! An error occurred during operation.");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}