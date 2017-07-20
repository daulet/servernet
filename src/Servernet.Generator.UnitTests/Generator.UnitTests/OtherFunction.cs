using Microsoft.Azure.WebJobs;

namespace Servernet.Generator.UnitTests
{
    public class OtherFunction
    {
        [FunctionName("OtherFunction")]
        public static void Run(
            [TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
        {
        }
    }
}
