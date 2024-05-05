using Party.Domain;

namespace Party.Application
{
    public interface IServersListGateway
    {
        IEnumerable<Server> GetServersByCountry(string country);
        IEnumerable<Server> GetServers();
        IEnumerable<Server> GetServersByProtocol(string protocol);
    }
}