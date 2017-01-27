using AzureTable.ConsoleHost.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Servernet.SelfHost;
using Servernet.SelfHost.Azure.Table;

namespace AzureTable.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = new TableSource<CustomerTable>(
                new CustomerTable(),
                new TableQuery(),
                new CustomerTableEntityFactory());
            var processor = new PurchaseProcessor();
            var runner = new Runner<IsStoredIn<CustomerTable>>(source, processor);

            runner.Run();
        }
    }
}
