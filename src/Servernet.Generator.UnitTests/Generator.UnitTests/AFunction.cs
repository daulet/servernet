using Microsoft.Azure.WebJobs;

namespace Servernet.Generator.UnitTests
{
    public class AFunction
    {
        public static void Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
        {
        }
    }
}