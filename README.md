# ServerNET
*Azure Functions made easy.*

Declare your Azure Function triggers, inputs and outputs in code:
``` cs
public static void Run(
    [QueueTrigger("myqueue-items")] string myQueueItem,
    [Blob("samples-workitems/{queueTrigger}")] string myInputBlob,
    [Blob("samples-workitems/{queueTrigger}-Copy)")] out string myOutputBlob)
{
    // your code here
}
```
and then use servernet command line tool to generate Azure Functions release package:
```
servernet.CLI.exe -a assembly.dll -f MyNamespace.Example.Run -o release/path
```
that will generate *project.json* that is ready to deploy:
``` 
{
  "disabled": false,
  "scriptFile": "assembly.dll",
  "entryPoint": "MyNamespace.Example.Run",
  "bindings": [
    {
      "connection": "BlobInputOutputFunction_trigger_queue_myQueueItem",
      "direction": "in",
      "name": "myQueueItem",
      "queueName": "myqueue-items",
      "type": "queueTrigger"
    },
    {
      "connection": "BlobInputOutputFunction_input_blob_myInputBlob",
      "direction": "in",
      "name": "myInputBlob",
      "path": "samples-workitems/{queueTrigger}",
      "type": "blob"
    },
    {
      "connection": "BlobInputOutputFunction_output_blob_myOutputBlob",
      "direction": "out",
      "name": "myOutputBlob",
      "path": "samples-workitems/{queueTrigger}-Copy)",
      "type": "blob"
    }
  ]
}
```

## Using command line tool

Generate a single function: 
```
servernet.CLI.exe -a assembly.dll -f Namespace.Type.FunctionName -o release/path
```

Generate functions for all types decorated with [AzureFunction] in the assembly: 
```
servernet.CLI.exe -a assembly.dll -o release/path
```

## Using [AzureFunction] attribute

Use [AzureFunction] attribute to let command line tool to autodetect functions to generate ([see above](#using-command-line-tool)), or to override parameters like *Disabled* and *Name*:
```cs
[AzureFunction(Disabled = false, Name = "NameToUseInAzureFunctions")]
public class MyFunctionClass
{
    // currently expecting a single public static method defined
}
```

# Bindings

![Code + Events](./docs/code%2Bevents.jpg)

Azure Functions provides binding of your code to various events/triggers. What ServerNET provides is letting you to focus on your code and generate deployable Azure Functions package, including boilerplate like *function.json* with trigger/input/output bindings and release layout of the Azure Function. ServerNET also limits you to things that are supported by Azure Functions, e.g. according to [Azure Function guidelines](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook#httptrigger) when you want to setup a webhook trigger you can't use *methods* property that you'd normally use for HTTP trigger. ServerNET provides strongly typed parameterization of your triggers, input and output parameters. What can't be enforced in design time (i.e. at compile time) is enforced at generation time (using ServerNET CLI), before you deploy to Azure, which means if there is any problem with your function definition you'll find out as soon as possible.

This library supports all the same trigger, input and output parameters as Azure Functions. Few exceptions are patterns that are bad practice, e.g. *out HttpResponseMessage* parameter. Pick entry method/function (currently only *public static* methods are supported) for your logic, decorate it with provided attributes and ServerNET CLI will generate deployable Azure Functions for you:

| Kind | Trigger | Input | Output |
| ---- | :-----: | :---: | :----: |
| [HTTP](#http-and-webhook-bindings) | [✔](#http-trigger) | | [✔](#http-output) |
| [WebHook](#http-and-webhook-bindings) | [✔](#webhook-trigger) | | [✔](#http-output) |
| [Timer](#timer-bindings) | [✔](#timer-trigger) | | |
| [Storage Blob](#blob-bindings) | [✔](#blob-trigger) | [✔](#blob-input) | [✔](#blob-output) |
| [Storage Queue](#queue-bindings) | [✔](#queue-trigger) | | [✔](#queue-output) |
| [Storage Table](#table-bindings) | | [✔](#table-input) | [✔](#table-output) |

## HTTP and WebHook bindings

For complete documentation on how to use HTTP and WebHook parameters in Azure Functions see [official documentaion](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook). For complete example that uses HTTP request and response see [HttpOutputFunction](./samples/DocumentationSamples/HttpOutputFunction.cs).

### HTTP Trigger

Decorate your entry method parameter with [HttpTrigger] attribute ([sample](./samples/DocumentationSamples/HttpTriggerFunction.cs)). Below is the list of parameter types that can be used with [HttpTrigger] attribute:
* [HttpRequestMessage](https://msdn.microsoft.com/en-us/library/system.net.http.httprequestmessage(v=vs.118).aspx);
* Custom type, request body will be deserialized from JSON into your object;

### WebHook Trigger

Decorate your entry method parameter with [WebHookTrigger] attribute ([sample](./samples/DocumentationSamples/WebHookTriggerFunction.cs)). Works with the same parameter types as [HTTP Trigger](#http-trigger).

### HTTP Output

Decoreate your entry method (not parameter) with [HttpOutput] attribute ([sample](./samples/DocumentationSamples/HttpOutputFunction.cs)). Using *out HttpResponseMessage* parameter is bad practice, hence only return parameter is supported (including async option with Task\<HttpResponseMessage\>). Below is the list of parameter types that can be used with [HttpResponse] attribute:
* [HttpResponseMessage](https://msdn.microsoft.com/en-us/library/system.net.http.httpresponsemessage(v=vs.118).aspx), including async equivalent Task\<HttpResponseMessage\>;

## Timer bindings

For complete documentation on how to use Timer parameters in Azure Functions see [official documentaion](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer).

### Timer Trigger

Decorate your entry method parameter with \[[TimerTrigger](https://github.com/Azure/azure-webjobs-sdk-extensions/wiki/TimerTrigger)\] attribute ([sample](./samples/DocumentationSamples/TimerTriggerFunction.cs)). Below is the list of parameter types that can be used with [TimerTrigger] attribute:
* [TimerInfo](https://github.com/Azure/azure-webjobs-sdk-extensions/blob/master/src/WebJobs.Extensions/Extensions/Timers/TimerInfo.cs);

## Blob bindings

For complete documentation on how to use Blob parameters in Azure Functions see [official documentaion](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob).

### Blob Trigger

Decorate your entry method parameter with \[[BlobTrigger](https://github.com/Azure/azure-webjobs-sdk/blob/dev/src/Microsoft.Azure.WebJobs/BlobTriggerAttribute.cs)\] attribute ([sample](./samples/DocumentationSamples/BlobTriggerFunction.cs)). Below is the list of parameter types that can be used with [BlobTrigger] attribute:
* Custom type, blob will be deserialized from JSON into your object;
* [String](https://msdn.microsoft.com/en-us/library/system.string(v=vs.110).aspx);
* [TextReader](https://msdn.microsoft.com/en-us/library/system.io.textreader(v=vs.110).aspx);
* [Stream](https://msdn.microsoft.com/en-us/library/system.io.stream(v=vs.110).aspx);
* [ICloudBlob](https://msdn.microsoft.com/library/azure/microsoft.windowsazure.storage.blob.icloudblob.aspx);
* [CloudBlockBlob](https://msdn.microsoft.com/en-us/library/azure/microsoft.windowsazure.storage.blob.cloudblockblob.aspx);
* [CloudPageBlob](https://msdn.microsoft.com/en-us/library/azure/microsoft.windowsazure.storage.blob.cloudpageblob.aspx);
* [CloudBlobContainer](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.storage.blob.cloudblobcontainer.aspx);
* [CloudBlobDirectory](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.storage.blob.cloudblobdirectory.aspx);
* IEnumerable<[CloudBlockBlob](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.storage.blob.cloudblockblob.aspx)>;
* IEnumerable<[CloudPageBlob](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.storage.blob.cloudpageblob.aspx)>;
* Other types deserialized [ICloudBlobStreamBinder](https://docs.microsoft.com/en-us/azure/app-service-web/websites-dotnet-webjobs-sdk-storage-blobs-how-to#icbsb);

### Blob Input

Decorate your entry method parameter with \[[Blob](https://github.com/Azure/azure-webjobs-sdk/blob/dev/src/Microsoft.Azure.WebJobs/BlobAttribute.cs)\] attribute ([sample](./samples/DocumentationSamples/BlobInputOutputFunction.cs)). Below is the list of parameter types that can be used with [Blob] attribute:
* Custom type, blob will be deserialized from JSON into your object;
* [String](https://msdn.microsoft.com/en-us/library/system.string(v=vs.110).aspx);
* [TextReader](https://msdn.microsoft.com/en-us/library/system.io.textreader(v=vs.110).aspx);
* [Stream](https://msdn.microsoft.com/en-us/library/system.io.stream(v=vs.110).aspx);
* [ICloudBlob](https://msdn.microsoft.com/library/azure/microsoft.windowsazure.storage.blob.icloudblob.aspx);
* [CloudBlockBlob](https://msdn.microsoft.com/en-us/library/azure/microsoft.windowsazure.storage.blob.cloudblockblob.aspx);
* [CloudPageBlob](https://msdn.microsoft.com/en-us/library/azure/microsoft.windowsazure.storage.blob.cloudpageblob.aspx);

### Blob Output

Decorate your entry method *out* parameter with \[[Blob](https://github.com/Azure/azure-webjobs-sdk/blob/dev/src/Microsoft.Azure.WebJobs/BlobAttribute.cs)\] attribute ([sample](./samples/DocumentationSamples/BlobInputOutputFunction.cs)). Below is the list of parameter types that can be used with [Blob] attribute:
* Custom type, blob will be deserialized from JSON into your object;
* [String](https://msdn.microsoft.com/en-us/library/system.string(v=vs.110).aspx);
* [TextReader](https://msdn.microsoft.com/en-us/library/system.io.textreader(v=vs.110).aspx);
* [Stream](https://msdn.microsoft.com/en-us/library/system.io.stream(v=vs.110).aspx);
* [CloudBlobStream](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.storage.blob.cloudblobstream.aspx);
* [ICloudBlob](https://msdn.microsoft.com/library/azure/microsoft.windowsazure.storage.blob.icloudblob.aspx);
* [CloudBlockBlob](https://msdn.microsoft.com/en-us/library/azure/microsoft.windowsazure.storage.blob.cloudblockblob.aspx);
* [CloudPageBlob](https://msdn.microsoft.com/en-us/library/azure/microsoft.windowsazure.storage.blob.cloudpageblob.aspx);

## Queue bindings

For complete documentation on how to use Queue parameters in Azure Functions see [official documentaion](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue).

### Queue Trigger

Decorate your entry method parameter with \[[QueueTrigger](hhttps://github.com/Azure/azure-webjobs-sdk/blob/dev/src/Microsoft.Azure.WebJobs/QueueTriggerAttribute.cs)\] attribute ([sample](./samples/DocumentationSamples/QueueTriggerFunction.cs)). Below is the list of parameter types that can be used with [QueueTrigger] attribute:
* T, where T is the type that you want to deserialize the queue message into as JSON;
* [String](https://msdn.microsoft.com/en-us/library/system.string(v=vs.110).aspx);
* [byte](https://msdn.microsoft.com/en-us/library/5bdb6693.aspx)[];
* [CloudQueueMessage](https://msdn.microsoft.com/library/azure/microsoft.windowsazure.storage.queue.cloudqueuemessage.aspx);

### Queue Output

Decorate your entry method *out* parameter with \[[Queue](hhttps://github.com/Azure/azure-webjobs-sdk/blob/dev/src/Microsoft.Azure.WebJobs/QueueTriggerAttribute.cs)\] attribute ([sample](./samples/DocumentationSamples/QueueOutputFunction.cs)). Below is the list of parameter types that can be used with [Queue] attribute:
* out T, where T is the type that you want to deserialize as JSON into queue message;
* out [String](https://msdn.microsoft.com/en-us/library/system.string(v=vs.110).aspx);
* out [byte](https://msdn.microsoft.com/en-us/library/5bdb6693.aspx)[];
* out [CloudQueueMessage](https://msdn.microsoft.com/library/azure/microsoft.windowsazure.storage.queue.cloudqueuemessage.aspx);
* ICollector\<T\> or IAsyncCollector\<T\>, where T is one of the supported types;

## Table bindings

For complete documentation on how to use Table parameters in Azure Functions see [official documentaion](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-table).

### Table Input

Decorate your entry method parameter with \[[Table](https://github.com/Azure/azure-webjobs-sdk/blob/dev/src/Microsoft.Azure.WebJobs/TableAttribute.cs)\] attribute ([sample](./samples/DocumentationSamples/TableInputFunction.cs)). Below is the list of parameter types that can be used with [Table] attribute:
* T, where T is the data type that you want to deserialize the data into;
* Any type that implements [ITableEntity](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.storage.table.itableentity.aspx);
* IQueryable\<T\>, where T is the data type that you want to deserialize the data into;

### Table Output

Decorate your entry method parameter with \[[Table](https://github.com/Azure/azure-webjobs-sdk/blob/dev/src/Microsoft.Azure.WebJobs/TableAttribute.cs)\] attribute ([sample](./samples/DocumentationSamples/TableOutputFunction.cs)). Below is the list of parameter types that can be used with [Table] attribute:
* out \<T\>, where T is the data type that you want to serialize the data into;
* out \<T\>, where T implements [ITableEntity](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.storage.table.itableentity.aspx);
* ICollector\<T\>, where T is the data type that you want to serialize the data into;
* IAsyncCollector\<T\> (async version of ICollector\<T\>);
* [CloudTable](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.storage.table.cloudtable.aspx);
