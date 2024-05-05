using Microsoft.EntityFrameworkCore;
using Party.Domain;

namespace Party.Infrastructure
{
    public class ServersDbContext(DbContextOptions<ServersDbContext> options) : DbContext(options)
    {
        public DbSet<Server> Servers { get; init; }
    }
}
