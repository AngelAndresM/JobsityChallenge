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
    public class StockRequestQueueConsumer
    {
        private readonly StockResponseQueueProducer _producer;
        private readonly IStockPriceHandler _priceHandler;
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;

        public StockRequestQueueConsumer(IConfiguration configuration, StockResponseQueueProducer producer, IStockPriceHandler priceHandler)
        {
            _configuration = configuration;
            var stockHostName = _configuration.GetConnectionString("StocksQueueConnection");

            _producer = producer;
            _priceHandler = priceHandler;

            _factory = new ConnectionFactory() { HostName = stockHostName };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Start()
        {
            _channel.QueueDeclare(queue: ApplicationConstants.StockQueueRequest,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var bytes = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(bytes);

                Console.WriteLine("-> Received {0}", message);

                var stockInfo = await _priceHandler.GetStockInfo(message);

                var result = JsonConvert.SerializeObject(stockInfo);

                _producer.SendStockInfo(stockInfo);
            };

            _channel.BasicConsume(queue: ApplicationConstants.StockQueueRequest,
                                 autoAck: true,
                                 consumer: consumer);

        }
    }
}
