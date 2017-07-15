namespace Servernet.Generator.Definition
{
    public interface IBinding
    {
        BindingDirection Direction { get; }

        BindingType Type { get; }
    }
}
