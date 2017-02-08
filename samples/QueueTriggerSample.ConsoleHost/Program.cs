using Microsoft.Azure.WebJobs;

namespace QueueTriggerSample.ConsoleHost
{
    public class Program
    {
        public static void Main()
        {
            var config = new JobHostConfiguration
            {
                TypeLocator = new SamplesTypeLocator(
                    typeof(Function))
            };
            //config.UseLogging();

            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
