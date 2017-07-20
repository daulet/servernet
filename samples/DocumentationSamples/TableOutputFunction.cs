using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.DocumentationSamples
{
    public class TableOutputFunction
    {
        [FunctionName("TableOutputFunction")]
        public static void Run(
            [QueueTrigger("myqueue-items")]string input,
            [Table("Person")] ICollector<Person> tableBinding,
            TraceWriter log)
        {
            for (var i = 1; i < 10; i++)
            {
                log.Info($"Adding Person entity {i}");
                tableBinding.Add(
                    new Person()
                    {
                        PartitionKey = "Test",
                        RowKey = i.ToString(),
                        Name = "Name" + i
                    }
                );
            }
        }

        public class Person
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public string Name { get; set; }
        }
    }
}
