using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

using TinyCsvParser;
using TinyCsvParser.Mapping;

using JobsityChat.Core.Contracts;
using JobsityChat.Core.Models;

namespace JobsityChat.Infraestructure.Services
{
    public class CsvStockPriceHandler : IStockPriceHandler
    {
        private const string Url = "​https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";

        public async Task<StockRecordInfo> GetStockInfo(string stockCode)
        {
            using (var client = new HttpClient())
            {
                var requestUrl = string.Format(Url, stockCode);
                var responseContent = await client.GetStreamAsync(requestUrl);

                var parser = new CsvParser<StockRecordInfo>(new CsvParserOptions(true, ','), new StockInfoCsvMapping());
                var records = parser.ReadFromStream(responseContent, Encoding.UTF8).ToList();
                var first = records.FirstOrDefault().Result;

                return first;
            }
        }
    }


    #region CsvMappings
    public class StockInfoCsvMapping : CsvMapping<StockRecordInfo>
    {
        public StockInfoCsvMapping()
            : base()
        {
            MapProperty(0, t => t.Symbol);
            MapProperty(1, t => t.Date);
            MapProperty(2, t => t.Time);
            MapProperty(3, t => t.Open);
            MapProperty(4, t => t.High);
            MapProperty(5, t => t.Low);
            MapProperty(6, t => t.Close);
            MapProperty(7, t => t.Volume);
        }
    }
    #endregion
}
