using System;
using System.Text;

using Microsoft.Extensions.Configuration;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

using JobsityChat.Core.Helpers;
using JobsityChat.Core.Models;
using JobsityChat.Core.Contracts;

namespace JobsityChat.StocksBot.RabbitMQ
{
    public class StockResponseQueueProducer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IConfiguration _configuration;

        public StockResponseQueueProducer(IConfiguration configuration)
        {
            _configuration = configuration;
            string hostName = _configuration.GetConnectionString("StocksQueueConnection");

            _factory = new ConnectionFactory() { HostName = hostName };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendStockInfo(StockRecordInfo stockInfo)
        {
            _channel.QueueDeclare(queue: ApplicationConstants.StockQueueResponse,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var stockJsonString = JsonConvert.SerializeObject(stockInfo);

            var body = Encoding.UTF8.GetBytes(stockJsonString);

            _channel.BasicPublish(exchange: "",
                                 routingKey: ApplicationConstants.StockQueueResponse,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("-> Sent {0}", stockJsonString);
        }
    }
}
