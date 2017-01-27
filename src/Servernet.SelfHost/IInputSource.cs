using System.Collections.Generic;

namespace Servernet.SelfHost
{
    public interface IInputSource<TInput>
        where TInput : IInput
    {
        IEnumerable<TInput> GetEnumerable();

        void MarkAsDone(TInput input);
    }
}
