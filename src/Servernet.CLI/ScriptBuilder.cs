using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Servernet.CLI
{
    internal class ScriptBuilder
    {
        private readonly Type _functionType;
        private readonly MethodInfo _method;
        private readonly IList<ParameterInfo> _parameters;

        public ScriptBuilder(Type functionType, MethodInfo method)
        {
            _functionType = functionType;
            _method = method;
            _parameters = new List<ParameterInfo>();
        }

        public void AddParameter(ParameterInfo parameter)
        {
            _parameters.Add(parameter);
        }

        public void WriteToStream(StreamWriter streamWriter)
        {
            var usingNamespaces = new HashSet<string>();

            var parameterList = new List<string>();
            foreach (var parameter in _parameters)
            {
                usingNamespaces.Add(parameter.ParameterType.Namespace);
                parameterList.Add($"{parameter.ParameterType.Name} {parameter.Name}");
            }

            // emit using statements
            foreach (var usingNamespace in usingNamespaces)
            {
                streamWriter.WriteLine($"using {usingNamespace};");
            }

            // empty line to separate using statements from function
            streamWriter.WriteLine();
            // @TODO handle non-void methods
            streamWriter.WriteLine($"public static void Run({string.Join(",", parameterList)}, TraceWriter log)");
            streamWriter.WriteLine("{");
            streamWriter.WriteLine($"\tvar instance = new {_functionType.Name}();");
            streamWriter.WriteLine($"\tinstance.{_method.Name}({string.Join(",", _parameters.Select(x => x.Name))});");
            streamWriter.WriteLine("}");
        }
    }
}
