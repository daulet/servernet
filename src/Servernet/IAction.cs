namespace Servernet
{
    public interface IAction<in TInput>
    {
        void Run(TInput input);
    }
}
