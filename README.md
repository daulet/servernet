# ServerNET

![Code + Events](.\docs\code+events.jpg)

Azure Functions provides binding of your code to events, what ServerNET provides is letting you to focus on your code and generate all Azure Functions boilerplate like *function.json* with trigger/input/output bindings and function release layout.

## Azure Functions HTTP and webhook bindings

This library supports all the same parameter types as Azure Function as documented [here](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook). The only exception to supported parameters are ones that are bad practice, e.g. out HttpResponseMessage parameter. For complete example that uses HTTP request and response see [TableEntityProcessorHttpTrigger](./samples/MultiTriggerSample/Trigger/TableEntityProcessorHttpTrigger.cs).

### HTTP Trigger

Decorate your entry method parameter with [HttpTrigger] attribute ([sample](./samples/MultiTriggerSample/Trigger/TableEntityProcessorHttpTrigger.cs)). Here is a list of parameter types that are supported with [HttpTrigger] attribute:
* [HttpRequestMessage](https://msdn.microsoft.com/en-us/library/system.net.http.httprequestmessage(v=vs.118).aspx);
* Custom type, request body will be deserialized from JSON into your object;

### HTTP Output

Decoreate your entry method (not parameter) with [HttpResponse] attribute ([sample](./samples/MultiTriggerSample/Trigger/TableEntityProcessorHttpTrigger.cs)). Using out HttpResponseMessage parameter is bad practice, hence only return parameter is supported (including async option with Task<HttpResponseMessage>). Here is a list of parameter types that are supported with [HttpResponse] attribute:
* [HttpResponseMessage](https://msdn.microsoft.com/en-us/library/system.net.http.httpresponsemessage(v=vs.118).aspx), including async equivalent Task<HttpResponseMessage>;