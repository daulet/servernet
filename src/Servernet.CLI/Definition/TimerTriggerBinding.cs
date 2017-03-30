using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class TimerTriggerBinding : IBinding
    {
        internal TimerTriggerBinding(string paramName, TimerTriggerAttribute attribute)
        {
            Direction = "in";
            Name = paramName;
            Schedule = attribute.ScheduleExpression;
        }

        public string Direction { get; }

        public string Name { get; set; }

        public string Schedule { get; set; }

        public BindingType Type { get; } = BindingType.TimerTrigger;
    }
}
