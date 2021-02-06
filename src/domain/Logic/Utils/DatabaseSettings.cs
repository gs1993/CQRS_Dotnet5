namespace Logic.Utils
{
    public sealed class DatabaseSettings
    {
        public int? NumberOfDatabaseRetries { get; set; }
        public string ConnectionString { get; set; }
        public string QueriesConnectionString { get; set; }
    }
}
