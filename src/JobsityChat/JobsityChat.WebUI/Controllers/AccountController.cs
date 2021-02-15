using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using JobsityChat.WebUI.Models;

namespace JobsityChat.WebUI.Controllers
{
    public class AccountController : Controller
    {
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
                //var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                //if (result.Succeeded)
                //{
                //    return RedirectToAction("Index", "Home");
                //}

                //ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(user);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = new IdentityUser
                //{
                //    UserName = model.Email,
                //    Email = model.Email,
                //};

                //var result = await _userManager.CreateAsync(user, model.Password);

                //if (result.Succeeded)
                //{
                //    await _signInManager.SignInAsync(user, isPersistent: false);

                //    return RedirectToAction("index", "Home");
                //}

                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError("", error.Description);
                //}

                //ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }
    }
}