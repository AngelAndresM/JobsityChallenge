using System;

namespace JobsityChat.Core.Contracts
{
    public interface IStockQueueProducer
    {
        void RequestStockInfo(string stockCode);
    }
}
