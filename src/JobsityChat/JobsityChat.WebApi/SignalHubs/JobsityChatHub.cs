using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

using JobsityChat.Core.Models;
using JobsityChat.Core.Contracts;
using JobsityChat.Core.Helpers;
using JobsityChat.WebApi.Models;
using JobsityChat.Infraestructure.Services;

namespace JobsityChat.WebApi.SignalHubs
{
    public class JobsityChatHub : Hub
    {
        private readonly UserManager<UserInfo> _userManager;
        private readonly IChatRoomCommandHandler _chatRoomCommandHandler;
        private readonly IMessageRepository _messageRepository;
        private readonly IStockQueueProducer _stockQueueProducer;

        public JobsityChatHub(UserManager<UserInfo> userManager, IMessageRepository messageRepository, IChatRoomCommandHandler chatRoomCommandHandler,
                              IStockQueueProducer stockQueueProducer)
        {
            _userManager = userManager;
            _messageRepository = messageRepository;
            _chatRoomCommandHandler = chatRoomCommandHandler;
            _stockQueueProducer = stockQueueProducer;
        }

        public async Task SendMessage(ChatMessageViewModel messageItem)
        {
            if (_chatRoomCommandHandler.IsCommand(messageItem.Message))
            {
                _chatRoomCommandHandler.ExecuteCommand(messageItem.Message, (commandName, commandParameter) =>
                {
                    //The 'messageItem' is a command message.

                    switch (commandName)
                    {
                        case "stock":
                            _stockQueueProducer.RequestStockInfo(commandParameter);
                            break;
                    }
                });
            }
            else
            {
                //Save message to database
                await SaveMessage(messageItem);
            }

            await Clients.All.SendAsync(ApplicationConstants.RECEIVE_MESSAGE, messageItem);
        }

        private async Task SaveMessage(ChatMessageViewModel item)
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            var repository = _messageRepository as MessageRepository;

            await repository.InsertAsync(new UserMessage
            {
                Message = item.Message,
                UserId = user.Id,
                CreationDate = DateTime.Parse(item.CreatedAt)
            });
        }
    }
}
