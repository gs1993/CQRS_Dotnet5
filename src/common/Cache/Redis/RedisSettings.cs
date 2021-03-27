namespace Cache.Redis
{
    public record RedisSettings
    {
        public string Server { get; init; }
        public string Port { get; init; }
        public string InstanceName { get; init; }
    }
}
