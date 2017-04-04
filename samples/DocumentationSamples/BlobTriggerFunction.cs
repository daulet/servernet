using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.DocumentationSamples
{
    [AzureFunction]
    public class BlobTriggerFunction
    {
        public static void Run(
            [BlobTrigger("samples-workitems")] string myBlob,
            TraceWriter log)
        {
            log.Info($"C# Blob trigger function processed: {myBlob}");
        }
    }
}