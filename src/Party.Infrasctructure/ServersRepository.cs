using Party.Application;
using Party.Domain;

namespace Party.Infrastructure
{
    internal class ServersRepository : IServersRepository
    {
        private readonly ServersDbContext _context;

        public ServersRepository(ServersDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Server> GetServers()
        {
            return _context.Servers.ToList();
        }
    }
}
