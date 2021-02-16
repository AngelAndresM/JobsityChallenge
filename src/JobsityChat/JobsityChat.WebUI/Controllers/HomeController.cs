using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JobsityChat.WebUI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using JobsityChat.WebUI.Services;
using JobsityChat.WebUI.Models.Response;
using Newtonsoft.Json;

namespace JobsityChat.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IJobsityApi _jobsityApi;

        public HomeController(ILogger<HomeController> logger, IJobsityApi jobsityApi)
        {
            _logger = logger;
            _jobsityApi = jobsityApi;
        }

        public async Task<IActionResult> Index()
        {
            var tokenString = GetUserToken();
            var messages = await GetLastMessageAsync($"Bearer {tokenString}");

            return View(messages);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        async Task<List<ChatMessageResponseModel>> GetLastMessageAsync(string token)
        {
            var items = new List<ChatMessageResponseModel>();

            try
            {
                var response = await _jobsityApi.GetLastMessagesAsync(token);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    items = JsonConvert.DeserializeObject<List<ChatMessageResponseModel>>(responseString);
                }
            }
            catch
            {

            }

            return items;
        }

        string GetUserToken()
        {
            var tokenString = string.Empty;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                tokenString = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication).Value;
            }

            return tokenString;
        }
    }
}
