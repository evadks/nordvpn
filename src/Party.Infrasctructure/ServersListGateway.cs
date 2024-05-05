using Newtonsoft.Json;
using Party.Application;
using Party.Domain;

namespace Party.Infrastructure
{
    internal class ServersListGateway(HttpClient httpClient) : IServersListGateway
    {
        private readonly Dictionary<string, byte> _countryNameToCodeMapping = new()
        {
            {"france", 74},
            {"albania", 2},
            {"argentina", 10}
        };

        private readonly Dictionary<string, byte> _protocolNameToCodeMapping = new()
        {
            {"udp", 3},
            {"tcp", 5}
        };

        public IEnumerable<Server> GetServersByCountry(string country)
        {
            var countryId = _countryNameToCodeMapping[country.ToLower()];
            var url = $"v1/servers?filters[servers_technologies][id]=35&filters[country_id]={countryId}";
            return FetchFromServer(url);
        }

        public IEnumerable<Server> GetServers()
        {
            return FetchFromServer("/v1/servers");
        }

        public IEnumerable<Server> GetServersByProtocol(string protocol)
        {
            var protocolId = _protocolNameToCodeMapping[protocol.ToLower()];
            var url = $"v1/servers?filters[servers_technologies][id]={protocolId}";
            return FetchFromServer(url);
        }

        private IEnumerable<Server> FetchFromServer(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = httpClient.SendAsync(request).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            var serversList = JsonConvert.DeserializeObject<List<Server>>(responseString);

            return serversList ?? [];
        }
    }
}
