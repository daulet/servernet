namespace Servernet
{
    public interface IAction<in TInput>
        where TInput : IInput
    {
        void Run(TInput input);
    }
}
