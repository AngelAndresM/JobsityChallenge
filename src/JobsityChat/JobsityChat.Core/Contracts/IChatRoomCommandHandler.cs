using System;

namespace JobsityChat.Core.Contracts
{
    public interface IChatRoomCommandHandler
    {
        bool IsCommand(string message);
        void ExecuteCommand(string message, Action<string, string> action);
    }
}
