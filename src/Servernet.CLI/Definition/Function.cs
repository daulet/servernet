using System.Collections.Generic;

namespace Servernet.CLI.Definition
{
    public class Function
    {
        public bool Disabled { get; set; }

        public IList<IBinding> Bindings { get; set; }
    }
}
