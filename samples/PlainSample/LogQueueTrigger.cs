using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Servernet.Samples.PlainSample
{
    public class LogQueueTrigger
    {
        public static void Run(
            [QueueTrigger("plain-queue")] CloudQueueMessage message,
            TraceWriter traceWriter)
        {
            traceWriter.Info($"Received {message.AsString}");
        }
    }
}
