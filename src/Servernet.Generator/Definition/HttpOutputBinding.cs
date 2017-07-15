namespace Servernet.Generator.Definition
{
    public class HttpOutputBinding : IBinding
    {
        public HttpOutputBinding(string paramName)
        {
            Name = paramName;
        }

        public BindingDirection Direction { get; } = BindingDirection.Out;

        public string Name { get; }

        public BindingType Type { get; } = BindingType.Http;
    }
}
