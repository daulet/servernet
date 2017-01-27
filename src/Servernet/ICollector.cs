namespace Servernet
{
    /// <summary>
    /// Interface for function return values
    /// </summary>
    public interface ICollector<in TOutput>
    {
        void Add(TOutput input);
    }
}
