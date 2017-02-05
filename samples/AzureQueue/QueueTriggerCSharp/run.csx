using System;

public static void Run(string myQueueItem, TraceWriter log, out string nextQueueItem)
{
    nextQueueItem = default(string);
    int currentValue;
    if (int.TryParse(myQueueItem, out currentValue))
    {
        if (currentValue <= 10)
        {
            currentValue++;
            nextQueueItem = currentValue.ToString();
        }
    }
}