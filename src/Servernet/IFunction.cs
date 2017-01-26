namespace Servernet
{
    public interface IFunction<in TInput>
        where TInput : IInput
    {
        void Run(TInput input);
    }
}
