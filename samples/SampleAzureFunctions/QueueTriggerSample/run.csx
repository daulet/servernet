#r "QueueTriggerSample.dll"
#r "Servernet.dll"
using System;
using QueueTriggerSample;
using QueueTriggerSample.Model;
using Servernet;

public static void Run(Purchase purchase, TraceWriter logger)
{
    Function.Run(purchase, logger);
}
