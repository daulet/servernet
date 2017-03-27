using Microsoft.WindowsAzure.Storage.Table;

namespace Servernet.Samples.MultiTriggerSample.Model
{
    public class TableSegment
    {
        public TableContinuationToken ContinuationToken { get; set; }
    }
}
