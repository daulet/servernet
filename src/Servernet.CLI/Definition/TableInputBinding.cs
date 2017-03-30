using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class TableInputBinding : IBinding
    {
        public TableInputBinding(string paramName, TableAttribute attribute)
        {
            Connection = "<Name of app setting that contains a storage connection string>";
            Direction = "in";
            Filter = attribute.Filter;
            Name = paramName;
            PartitionKey = attribute.PartitionKey;
            RowKey = attribute.RowKey;
            // @TODO enforce table name rules:
            // https://docs.microsoft.com/en-us/rest/api/storageservices/fileservices/Understanding-the-Table-Service-Data-Model
            TableName = attribute.TableName;
            Take = attribute.Take;
            Type = "table";
        }

        public string Connection { get; set; }
        public string Direction { get; set; }
        public string Filter { get; set; }
        public string Name { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string TableName { get; set; }
        public int Take { get; set; }
        public string Type { get; set; }
    }
}
