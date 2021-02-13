using System;
using System.Threading;

namespace JobsityChat.StocksBot
{
    class Program
    {
        static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            _closing.WaitOne();
        }
    }
}
