using Microsoft.Azure.WebJobs.Host;

namespace Servernet
{
    public class Logger
    {
        private readonly TraceWriter _traceWriter;

        public Logger(TraceWriter traceWriter)
        {
            _traceWriter = traceWriter;
        }

        public void Info(string message)
        {
            _traceWriter.Info(message);
        }
    }
}
