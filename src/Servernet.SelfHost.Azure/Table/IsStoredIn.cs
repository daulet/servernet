namespace Servernet.SelfHost.Azure.Table
{
    public interface IsStoredIn<TTableDefinition> : IInput
        where TTableDefinition : ITableDefinition
    {
    }
}
