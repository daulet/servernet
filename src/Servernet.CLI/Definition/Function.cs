using System.Collections.Generic;

namespace Servernet.CLI.Definition
{
    public class Function
    {
        public bool Disabled { get; set; }

        public string ScriptFile { get; set; }

        public string EntryPoint { get; set; }
        
        public IList<IBinding> Bindings { get; set; }
    }
}
