using System;
using System.IO;
using System.Threading;

using Microsoft.Extensions.Configuration;

using JobsityChat.Core.Contracts;
using JobsityChat.Infraestructure.Services;
using JobsityChat.StocksBot.RabbitMQ;

namespace JobsityChat.StocksBot
{
    class Program
    {
        static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            // setup appsettings files
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            // setup queue message handlers
            var producer = new StockResponseQueueProducer(configuration);
            var priceHandler = new CsvStockPriceHandler();
            var consumer = new StockRequestQueueConsumer(configuration, producer, priceHandler);

            // starts listening
            consumer.Start();

            Console.WriteLine("The programs up");
            Console.WriteLine("Waiting for command messages...");

            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);

            _closing.WaitOne();
        }

        private static void OnExit(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Exit");
            _closing.Set();
        }
    }
}
