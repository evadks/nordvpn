using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Party.Application;

namespace Party.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IServersListGateway, ServersListGateway>();
            services.AddSingleton<IServersRepository, InMemoryServersRepository>();
            services.AddDbContext<ServersDbContext>(options => options.UseInMemoryDatabase("ServersDb"));
            services.AddHttpClient<IServersListGateway, ServersListGateway>(c => c.BaseAddress = new Uri("https://api.nordvpn.com/"));
        }
    }
}
