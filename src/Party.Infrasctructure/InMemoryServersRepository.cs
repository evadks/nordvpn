
using Party.Application;
using Party.Domain;

namespace Party.Infrastructure
{
    internal class InMemoryServersRepository : IServersRepository
    {
        public IEnumerable<Server> GetServers()
        {
            var servers = new List<Server>();
            var server = new Server()
            {
                Name = "InMemoryProvider_Name",
                Load = 1,
                Status = "InMemoryProvider_Online"
            };
            servers.Add(server);
            return servers;
        }
    }
}
