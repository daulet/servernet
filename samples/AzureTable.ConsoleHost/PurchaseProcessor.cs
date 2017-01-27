using AzureTable.ConsoleHost.Model;
using AzureTable.ConsoleHost.Storage;
using Servernet;
using Servernet.SelfHost.Azure.Table;

namespace AzureTable.ConsoleHost
{
    public class PurchaseProcessor : IAction<IsStoredIn<CustomerTable>>
    {
        public void Run(IsStoredIn<CustomerTable> input)
        {
            var purchase = input as Purchase;
            if (purchase != null)
            {
                // process 
            }
        }
    }
}
