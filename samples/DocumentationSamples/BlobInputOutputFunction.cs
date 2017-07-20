using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.DocumentationSamples
{
    public class BlobInputOutputFunction
    {
        [FunctionName("BlobInputOutputFunction")]
        public static void Run(
            [QueueTrigger("myqueue-items")] string myQueueItem,
            [Blob("samples-workitems/{queueTrigger}")] string myInputBlob,
            [Blob("samples-workitems/{queueTrigger}-Copy)")] out string myOutputBlob,
            TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
            myOutputBlob = myInputBlob;
        }
    }
}