namespace Servernet.CLI.Definition
{
    public interface IBinding
    {
        BindingDirection Direction { get; }

        BindingType Type { get; }
    }
}
