using Servernet;

namespace AzureQueue.ConsoleHost
{
    public class PaymentProcessor : IFunction<PaymentMessage>
    {
        public void Run(PaymentMessage input)
        {
            throw new System.NotImplementedException();
        }
    }
}
