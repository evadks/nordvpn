namespace Party.Domain
{
    public class Server
    {
        public required string Name { get; init; }
        public required int Load { get; init; }
        public required string Status { get; init; }
    }
}
