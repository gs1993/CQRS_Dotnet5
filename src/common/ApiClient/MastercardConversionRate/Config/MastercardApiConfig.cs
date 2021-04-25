namespace ApiClient.MastercardConversionRate.Config
{
    public record MastercardApiConfig
    {
        public string ConsumerKey { get; init; }
        public string KeyAlias { get; init; }
        public string KeyPassword { get; init; }
        public string CertPath { get; init; }
        public string Url { get; init; }
        public int CurrencyRateServiceTimeoutInSeconds { get; init; }
        public int SettlementCurrenciesServiceTimeoutInSeconds { get; init; }
    }
}
