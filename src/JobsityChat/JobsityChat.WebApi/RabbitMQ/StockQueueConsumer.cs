using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

using JobsityChat.Core.Contracts;
using JobsityChat.Core.Helpers;
using JobsityChat.Core.Models;

using JobsityChat.WebApi.SignalHubs;
using JobsityChat.WebApi.Models;

namespace JobsityChat.WebApi.RabbitMQ
{
    public class StockQueueConsumer : BackgroundService, IStockQueueConsumer
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IHubContext<JobsityChatHub> _chatHub;
        private readonly IConfiguration _configuration;

        public StockQueueConsumer(IConfiguration configuration, IHubContext<JobsityChatHub> chatHub)
        {
            _configuration = configuration;
            var stockHostName = _configuration.GetConnectionString("StocksQueueConnection");

            _factory = new ConnectionFactory() { HostName = stockHostName };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: ApplicationConstants.StockQueueResponse,
                durable: false,
                exclusive: false,
                autoDelete: false);

            _chatHub = chatHub;
        }

        public void Start()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockInfo = JsonConvert.DeserializeObject<StockRecordInfo>(message);

                if (stockInfo != null)
                {
                    // Send message to chat
                    var responseMessage = $"{stockInfo.Symbol.ToUpper()} quote is {stockInfo.Close:F} per share";
                    await AnswerRequest(responseMessage);
                }
                else
                {
                    // Send message to chat
                    var responseMessage = "Sorry, we couldn't find the stock you're looking for.";
                    await AnswerRequest(responseMessage);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            // Consume a RabbitMQ Queue
            _channel.BasicConsume(queue: ApplicationConstants.StockQueueResponse, autoAck: false, consumer: consumer);
        }

        private async Task AnswerRequest(string responseMessage)
        {
            await _chatHub.Clients.All.SendAsync(ApplicationConstants.RECEIVE_MESSAGE, new ChatMessageViewModel
            {
                UserFullName = "Jobsity Chat Bot",
                UserName = ApplicationConstants.BotName,
                Message = responseMessage,
                CreatedAt = DateTime.Now.ToString("d")
            });
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            Start();

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();

            base.Dispose();
        }
    }
}
