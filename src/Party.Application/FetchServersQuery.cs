using MediatR;

namespace Party.Application
{
    public class FetchServersQuery(string country = null, string protocol = null, bool local = false)
        : IRequest<IEnumerable<ServerDto>>
    {
        internal string? Country { get; } = country;
        internal string?  Protocol { get; } = protocol;
        private bool Local { get; } = local;


        public bool HasFilterByCountry => !string.IsNullOrWhiteSpace(Country);
        public bool HasFilterByProtocol => !string.IsNullOrWhiteSpace(Protocol);
        public bool HasFilterByLocal => Local;
    }
}
