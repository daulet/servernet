using System;
using System.Threading.Tasks;

namespace Servernet.SelfHost
{
    public class Runner<TInput>
        where TInput : IInput
    {
        private readonly IInputSource<TInput> _inputSource;
        private readonly IFunction<TInput> _function;

        public Runner(IInputSource<TInput> inputSource, IFunction<TInput> function)
        {
            _inputSource = inputSource;
            _function = function;
        }

        public void Run()
        {
            Parallel.ForEach(_inputSource.GetEnumerable(), input =>
            {
                try
                {
                    _function.Run(input);
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
}
