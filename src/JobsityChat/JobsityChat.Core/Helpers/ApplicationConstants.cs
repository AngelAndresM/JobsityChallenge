using System;

namespace JobsityChat.Core.Helpers
{
    public class ApplicationConstants
    {
        //Hub
        public const string RECEIVE_MESSAGE = "ReceiveMessage";
        public const string BotName = "JobsityBot";

        //RabbitMQ
        public const string StockQueueRequest = "StockQueueRequest";
        public const string StockQueueResponse = "StockQueueResponse";

    }
}
