using System.Collections.Generic;

namespace Servernet.SelfHost
{
    public class ArraySource<TInput> : IInputSource<TInput>
    {
        private readonly TInput[] _inputs;

        public ArraySource(TInput[] inputs)
        {
            _inputs = inputs;
        }

        public IEnumerable<TInput> GetEnumerable()
        {
            return _inputs;
        }

        public void MarkAsDone(TInput input)
        {
        }
    }
}
