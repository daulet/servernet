namespace Servernet.Samples.MultiTriggerSample
{
    public interface IFunction<in TInput, out TOutput>
    {
        TOutput Run(TInput input);
    }
}
