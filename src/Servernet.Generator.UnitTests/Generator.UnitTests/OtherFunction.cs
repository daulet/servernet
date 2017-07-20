using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Net.Http;
using System.Threading.Tasks;

namespace Servernet.Generator.UnitTests
{
    public class OtherFunction
    {
        [FunctionName("OtherFunction")]
        public static Task Run(
            [HttpTrigger(HttpMethod.Get, "HttpTriggerFunction")] HttpRequestMessage req,
            TraceWriter log)
        {
            return Task.FromResult(0);
        }
    }
}
