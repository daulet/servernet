using Microsoft.Azure.WebJobs;

namespace Servernet.Generator.Definition
{
    public class TimerTriggerBinding : IBinding
    {
        internal TimerTriggerBinding(string paramName, TimerTriggerAttribute attribute)
        {
            Name = paramName;
            Schedule = attribute.ScheduleExpression;
        }

        public BindingDirection Direction { get; } = BindingDirection.In;

        public string Name { get; set; }

        public string Schedule { get; set; }

        public BindingType Type { get; } = BindingType.TimerTrigger;
    }
}
