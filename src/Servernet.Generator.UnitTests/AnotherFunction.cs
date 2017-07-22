using Microsoft.Azure.WebJobs;

namespace Servernet.Generator.UnitTests
{
    public class AnotherFunction
    {
        [FunctionName("YetAnotherFunction")]
        public static void Run(
            [TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
        {
        }
    }
}
