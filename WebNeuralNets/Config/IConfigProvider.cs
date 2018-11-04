namespace GroupProjectBackend.Config
{
    public interface IConfigProvider
    {
        string ConnectionString { get; }
    }
}