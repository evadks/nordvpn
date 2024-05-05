namespace Party.Application
{
    public class ServerDto(string name, int load, string status)
    {
        private string Name { get; } = name;
        private int Load { get; } = load;
        private string Status { get; } = status;

        public override string ToString()
        {
            return $"{Name, -25} | {Load, -5} | {Status, -7}";
        }
    }
}
