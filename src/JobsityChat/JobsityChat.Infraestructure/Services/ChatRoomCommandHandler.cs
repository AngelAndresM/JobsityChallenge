using System;

using JobsityChat.Core.Contracts;

namespace JobsityChat.Infraestructure.Services
{
    public class ChatRoomCommandHandler : IChatRoomCommandHandler
    {
        public bool IsCommand(string message)
        {
            return message.StartsWith("/") && message.Length > 1;
        }

        public void ExecuteCommand(string message, Action<string, string> action)
        {
            var commandText = message.Trim().Remove('/').ToLower();
            var commandParts = commandText.Split("=");

            var commandName = commandParts[0];
            var commandParameter = commandParts.Length < 2 ? string.Empty : commandParts[1];

            action(commandName, commandParameter);
        }
    }
}
