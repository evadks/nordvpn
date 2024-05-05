namespace Party.Infrastructure.Contracts
{
    public interface IServersRepository
    {
        IEnumerable<string> GetServers();
    }
}
