using AzureQueue.ConsoleHost.Model;
using Servernet;

namespace AzureQueue.ConsoleHost
{
    public class PaymentProcessor : IAction<Payment>
    {
        public void Run(Payment input)
        {
            throw new System.NotImplementedException();
        }
    }
}
