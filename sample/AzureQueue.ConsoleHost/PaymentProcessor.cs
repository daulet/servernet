using AzureQueue.ConsoleHost.Model;
using Servernet;

namespace AzureQueue.ConsoleHost
{
    public class PaymentProcessor : IFunction<Payment>
    {
        public void Run(Payment input)
        {
            throw new System.NotImplementedException();
        }
    }
}
