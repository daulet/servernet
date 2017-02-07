using Microsoft.Azure.WebJobs;

namespace QueueTriggerSample.ConsoleHost
{
    public class Program
    {
        public static void Main()
        {
            var config = new JobHostConfiguration();

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            var host = new JobHost(config);
            config.TypeLocator = new SamplesTypeLocator(
                    typeof(Function));

            host.RunAndBlock();
        }
    }
}
