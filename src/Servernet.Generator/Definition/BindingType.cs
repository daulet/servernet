﻿namespace Servernet.Generator.Definition
{
    internal enum Binding
    {
        Blob,
        Http,
        Queue,
        Table,
        Timer,
    }

    internal static class BindingCategory
    {
        public const int Trigger = 1 << 10;
    }

    public enum BindingType
    {
        Blob = Binding.Blob,
        BlobTrigger = Binding.Blob | BindingCategory.Trigger,
        Http = Binding.Http,
        HttpTrigger = Binding.Http | BindingCategory.Trigger,
        Queue = Binding.Queue,
        QueueTrigger = Binding.Queue | BindingCategory.Trigger,
        Table = Binding.Table,
        TimerTrigger = Binding.Timer | BindingCategory.Trigger,
    }
}