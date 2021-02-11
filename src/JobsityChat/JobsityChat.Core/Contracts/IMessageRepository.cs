using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using JobsityChat.Core.Models;

namespace JobsityChat.Core.Contracts
{
    public interface IMessageRepository
    {
        Task<List<UserMessage>> GetLastMessagesAsync(int count);
    }
}
