using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.DocumentationSamples
{
    public class QueueOutputFunction
    {
        public static void Run(
            [QueueTrigger("myqueue-input")] string input,
            [Queue("myqueue-output")] out string myQueueItem,
            TraceWriter log)
        {
            myQueueItem = "New message: " + input;
        }
    }
}