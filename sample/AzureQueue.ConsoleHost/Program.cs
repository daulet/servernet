using AzureQueue.ConsoleHost.Model;
using Servernet.SelfHost;
using Servernet.SelfHost.Azure.Queue;
using System;

namespace AzureQueue.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = new QueueSource<PaymentQueue>(
                new PaymentFactory(),
                new PaymentQueue(),
                TimeSpan.FromSeconds(10));
            var processor = new PaymentProcessor();
            var runner = new Runner<Payment>(source, processor);

            runner.Run();
        }
    }
}
