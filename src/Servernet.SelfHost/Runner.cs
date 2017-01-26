namespace Servernet.SelfHost
{
    public class Runner
    {
        public void Register<TInput>(IFunction<TInput> function)
            where TInput : IInput
        {

        }

        public void Run()
        { }
    }
}
