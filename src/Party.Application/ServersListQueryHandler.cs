using MediatR;

namespace Party.Application
{
    public class ServersListQueryHandler : IRequestHandler<FetchServersQuery, IEnumerable<ServerDto>>
    {
        private readonly IServersListGateway _serversListGateway;
        private readonly IServersRepository _serversRepository;

        public ServersListQueryHandler(IServersListGateway serversListGateway, IServersRepository serversRepository)
        {
            if (serversListGateway == null)
            {
                throw new ArgumentNullException($"{nameof(IServersListGateway)} must be set on Handler creation");
            }

            if (serversRepository == null)
            {
                throw new ArgumentNullException($"{nameof(IServersRepository)} must be set on Handler creation");
            }

            _serversListGateway = serversListGateway;
            _serversRepository = serversRepository;
        }

        public Task<IEnumerable<ServerDto>> Handle(FetchServersQuery request, CancellationToken cancellationToken)
        {
            if (request.HasFilterByCountry)
            {
                var servers = _serversListGateway.GetServersByCountry(request.Country);

                var serversDto = servers.Select(s => new ServerDto(s.Name, s.Load, s.Status));

                return Task.FromResult(serversDto);
            }

            if (request.HasFilterByProtocol)
            {
                var servers = _serversListGateway.GetServersByProtocol(request.Protocol);

                var serversDto = servers.Select(s => new ServerDto(s.Name, s.Load, s.Status));

                return Task.FromResult(serversDto);
            }

            if (request.HasFilterByLocal)
            {
                var servers = _serversRepository.GetServers();
                var serversDto = servers.Select(s => new ServerDto(s.Name, s.Load, s.Status));
                return Task.FromResult(serversDto);
            }
            else
            {
                var servers = _serversListGateway.GetServers();
                var serversDto = servers.Select(s => new ServerDto(s.Name, s.Load, s.Status));
                return Task.FromResult(serversDto);
            }
        }
    }
}
