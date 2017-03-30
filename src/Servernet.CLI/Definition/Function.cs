using System.Collections.Generic;
using Newtonsoft.Json;

namespace Servernet.CLI.Definition
{
    public class Function
    {
        [JsonIgnore]
        internal string Name { get; set; }

        public bool Disabled { get; set; }

        public string ScriptFile { get; set; }

        public string EntryPoint { get; set; }
        
        public IList<IBinding> Bindings { get; set; }
    }
}
