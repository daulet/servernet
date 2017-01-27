namespace Servernet
{
    public interface IFunction<in TInput, out TOutput>
    {
        TOutput Run(TInput input);
    }
}
