using Servernet.SelfHost.Core;
using System;
using System.Threading.Tasks;

namespace Servernet.SelfHost
{
    public class Runner<TInput>
        where TInput : IInput
    {
        private readonly IInputSource<TInput> _inputSource;
        private readonly IAction<TInput> _action;

        public Runner(IInputSource<TInput> inputSource, IAction<TInput> action)
        {
            _inputSource = inputSource;
            _action = action;
        }

        public void Run()
        {
            Parallel.ForEach(_inputSource.GetEnumerable(), input =>
            {
                try
                {
                    _action.Run(input);
                    _inputSource.MarkAsDone(input);
                }
                catch (Exception)
                {
                    // TODO perhaps UpdateMessage with retry count
                    // TODO some kind of retry logic
                    throw;
                }
            });
        }
    }

    public class Runner<TInput, TOutput> : Runner<TInput>
        where TInput : IInput
    {
        public Runner(IInputSource<TInput> inputSource, IFunction<TInput, TOutput> function, ICollector<TOutput> collector)
            : base(inputSource, new FunctionAsAction<TInput, TOutput>(function, collector))
        {
        }
    }
}
