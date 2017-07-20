using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.DocumentationSamples
{
    public class TableInputFunction
    {
        public static void Run(
            [QueueTrigger("myqueue-items")] string myQueueItem,
            [Table("Person", "Test", "{queueTrigger}")] Person personEntity,
            TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
            log.Info($"Name in Person entity: {personEntity.Name}");
        }

        public class Person
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public string Name { get; set; }
        }
    }
}