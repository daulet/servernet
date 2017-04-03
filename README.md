# ServerNET
*Azure Functions made easy.*

![Code + Events](./docs/code%2Bevents.jpg)

Azure Functions provides binding of your code to various events/triggers. What ServerNET provides is letting you to focus on your code and generate valid Azure Functions package, including boilerplate like *function.json* with trigger/input/output bindings and release layout of the Azure Function. ServerNET also limits you to things that are supported by Azure Functions, e.g. according to [Azure Function guidelines](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook#httptrigger) when you want to setup a webhook trigger you can't use *methods* property that you'd normally use for HTTP trigger. ServerNET provides strongly typed parameterization of your triggers, input and output parameters. What can't be enforced in design time (i.e. at compile time) is enforced at generation time (using ServerNET CLI), before you deploy to Azure, which means if there is any problem with your function definition you'll find out as soon as possible.

This library supports all the same trigger, input and output parameters as Azure Functions. Few exceptions are patterns that are bad practice, e.g. *out HttpResponseMessage* parameter.

## HTTP and WebHook bindings

For complete documentation on how to use HTTP and WebHook parameters in Azure Functions see [official documentaion](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook). For complete example that uses HTTP request and response see [HttpOutputFunction](./samples/DocumentationSamples/HttpOutputFunction.cs).

### HTTP Trigger

Decorate your entry method parameter with [HttpTrigger] attribute ([sample](./samples/DocumentationSamples/HttpTriggerFunction.cs)). Here is a list of parameter types that are supported with [HttpTrigger] attribute:
* [HttpRequestMessage](https://msdn.microsoft.com/en-us/library/system.net.http.httprequestmessage(v=vs.118).aspx);
* Custom type, request body will be deserialized from JSON into your object;

### WebHook Trigger

Decorate your entry method parameter with [WebHookTrigger] attribute ([sample](./samples/DocumentationSamples/WebHookTriggerFunction.cs)). Works with the same parameter types as [HTTP Trigger](#http-trigger).

### HTTP Output

Decoreate your entry method (not parameter) with [HttpOutput] attribute ([sample](./samples/DocumentationSamples/HttpOutputFunction.cs)). Using *out HttpResponseMessage* parameter is bad practice, hence only return parameter is supported (including async option with Task\<HttpResponseMessage\>). Below is the list of parameter types that are supported with [HttpResponse] attribute:
* [HttpResponseMessage](https://msdn.microsoft.com/en-us/library/system.net.http.httpresponsemessage(v=vs.118).aspx), including async equivalent Task\<HttpResponseMessage\>;

## Timer bindings

For complete documentation on how to use Timer parameters in Azure Functions see [official documentaion](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer).

### Timer Trigger

Decorate your entry method parameter with \[[TimerTrigger](https://github.com/Azure/azure-webjobs-sdk-extensions/wiki/TimerTrigger)\] attribute ([sample](./samples/DocumentationSamples/TimerTriggerFunction.cs)). Below is the list of parameter types that are supported with [TimerTrigger] attribute:
* [TimerInfo](https://github.com/Azure/azure-webjobs-sdk-extensions/blob/master/src/WebJobs.Extensions/Extensions/Timers/TimerInfo.cs);
