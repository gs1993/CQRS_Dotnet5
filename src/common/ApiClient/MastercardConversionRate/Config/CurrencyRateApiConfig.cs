namespace ApiClient.MastercardConversionRate.Config
{
    public record CurrencyRateApiConfig
    {
        public string ConsumerKey { get; init; }
        public string KeyAlias { get; init; }
        public string KeyPassword { get; init; }
        public string CertPath { get; init; }
        public string Url { get; init; }
    }
}
