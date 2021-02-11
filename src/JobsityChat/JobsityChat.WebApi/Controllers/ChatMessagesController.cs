using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

using JobsityChat.Core.Models;
using JobsityChat.Core.Contracts;
using JobsityChat.WebApi.Models;

namespace JobsityChat.WebApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatMessagesController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMessageRepository _messageRepository;

        public ChatMessagesController(ILogger<UserController> logger, IMessageRepository messageRepository)
        {
            _logger = logger;
            _messageRepository = messageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLastMessages()
        {
            var numberOfMessages = 50;
            var messages = await _messageRepository.GetLastMessagesAsync(numberOfMessages);
            var result = new List<ChatMessageViewModel>();

            result = messages.Select(t => new ChatMessageViewModel
            {
                UserName = t.User.UserName,
                UserFullName = $"{t.User.FirstName} {t.User.LastName}",
                Message = t.Message,
                CreatedAt = t.CreationDate.ToString("d")
            }).ToList();

            return Ok(result);
        }
    }
}
