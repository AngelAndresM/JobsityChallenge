using System;
using System.Text;

using RabbitMQ.Client;

using JobsityChat.Core.Helpers;
using JobsityChat.Core.Contracts;
using Microsoft.Extensions.Configuration;

namespace JobsityChat.WebApi.RabbitMQ
{
    public class StockQueueProducer : IStockQueueProducer
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;

        public StockQueueProducer(IConfiguration configuration)
        {
            _configuration = configuration;
            var stockHostName = _configuration.GetConnectionString("StocksQueueConnection");

            _factory = new ConnectionFactory() { HostName = stockHostName };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void RequestStockInfo(string stockCode)
        {
            _channel.QueueDeclare(queue: ApplicationConstants.StockQueueRequest,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var messageBody = Encoding.UTF8.GetBytes(stockCode);

            _channel.BasicPublish(exchange: "", routingKey: ApplicationConstants.StockQueueRequest, body: messageBody, basicProperties: null);
        }
    }
}
