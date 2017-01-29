using System.Collections.Generic;

namespace Servernet.SelfHost
{
    public interface IInputSource<TInput>
    {
        IEnumerable<TInput> GetEnumerable();

        void MarkAsDone(TInput input);
    }
}
