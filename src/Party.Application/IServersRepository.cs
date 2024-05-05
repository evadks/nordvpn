using Party.Domain;

namespace Party.Application
{
    public interface IServersRepository
    {
        IEnumerable<Server> GetServers();
    }
}