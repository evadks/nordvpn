using System.ComponentModel;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Party.Application;
using Party.Infrastructure;

namespace Party.Presentation.CLI
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });

            AddConfiguration(builder);
            AddApplicationServices(builder);
            AddInfrastructureServices(builder);
            AddLogging(builder);

            var host = builder.Build();

            await Run(args, host.Services.GetRequiredService<IMediator>());
        }

        private static void AddInfrastructureServices(IHostBuilder builder)
        {
            builder.ConfigureServices((_, s) => s.AddInfrastructureServices());
        }

        private static async Task Run(string[] args, ISender mediator)
        {
            args = TrimArguments(args);

            var protocol = ExtractProtocol(args);

            var country = ExtractCountryFrom(args);

            var local = ExtractLocalParameterFrom(args);

            var fsq = new FetchServersQuery(country, protocol, local);

            var servers = await mediator.Send(fsq);

            DisplayList(servers);
        }

        private static string[] TrimArguments(IEnumerable<string> args)
        {
            return args.Select(arg => arg.StartsWith("--") ? arg[2..] : arg).ToArray();
        }

        private static bool ExtractLocalParameterFrom(IEnumerable<string> args)
        {
            return args.Contains("local");
        }

        private static string? ExtractCountryFrom(IEnumerable<string> args)
        {
            var countryList = Enum.GetValues(typeof(CountryValue))
                .Cast<Enum>()
                .Select(enumValue => enumValue.GetType()
                    .GetField(enumValue.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute)
                .Where(attr => attr != null)
                .Select(attr => $"{attr.Description}")
                .ToArray();

            return countryList
                    .Intersect(args, StringComparer.CurrentCultureIgnoreCase)
                    .FirstOrDefault();
        }

        private static string? ExtractProtocol(IEnumerable<string> args)
        {
            var protocolNames = Enum.GetNames(typeof(ProtocolValues));

            return protocolNames
                .Intersect(args, StringComparer.CurrentCultureIgnoreCase)
                .FirstOrDefault();
        }

        private static void DisplayList(IEnumerable<ServerDto> servers)
        {
            Console.WriteLine("Server list: ");

            foreach (var server in servers)
            {
                Console.WriteLine(server);
            }

            Console.WriteLine("Total servers: " + servers.Count());
        }

        private static void AddLogging(IHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });
        }

        private static void AddApplicationServices(IHostBuilder builder)
        {
            builder.ConfigureServices((_, s) => s.AddApplicationServices());
        }

        private static void AddConfiguration(IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, configuration) =>
            {
                configuration.Sources.Clear();
                configuration
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
            });
        }
    }
}
