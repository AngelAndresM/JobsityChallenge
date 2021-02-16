using System;
using System.Net.Http;
using System.Threading.Tasks;
using JobsityChat.WebUI.Models.Request;
using Refit;

namespace JobsityChat.WebUI.Services
{
    public interface IJobsityApi
    {
        [Post("/api/authentication/login")]
        Task<HttpResponseMessage> LoginAsync([Body] LoginRequestModel model);

        [Post("/api/user/register")]
        Task<HttpResponseMessage> RegisterAsync([Body] RegisterRequestModel model);
    }
}
