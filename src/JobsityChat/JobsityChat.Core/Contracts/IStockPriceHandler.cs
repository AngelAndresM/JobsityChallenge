using System;
using System.Threading.Tasks;

using JobsityChat.Core.Models;

namespace JobsityChat.Core.Contracts
{
    public interface IStockPriceHandler
    {
        Task<StockRecordInfo> GetStockInfo(string stockCode);
    }
}
