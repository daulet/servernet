namespace Servernet.SelfHost.Core
{
    internal class FunctionAsAction<TInput, TOutput> : IAction<TInput>
    {
        private readonly IFunction<TInput, TOutput> _function;
        private readonly ICollector<TOutput> _collector;

        public FunctionAsAction(IFunction<TInput, TOutput> function, ICollector<TOutput> collector)
        {
            _function = function;
            _collector = collector;
        }

        public void Run(TInput input)
        {
            var result = _function.Run(input);
            _collector.Add(result);
        }
    }
}
