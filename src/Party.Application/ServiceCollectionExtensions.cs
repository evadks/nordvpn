using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Party.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var execAsm = Assembly.GetExecutingAssembly();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(execAsm));
        }
    }
}
