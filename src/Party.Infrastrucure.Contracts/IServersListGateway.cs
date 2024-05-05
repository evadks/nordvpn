namespace Party.Infrastructure.Contracts
{
    public interface IServersListGateway
    {
        IEnumerable<string> GetServersByCountry(string country);
        IEnumerable<string> GetServers();
        IEnumerable<string> GetServersByProtocol(string protocol);
    }
}
