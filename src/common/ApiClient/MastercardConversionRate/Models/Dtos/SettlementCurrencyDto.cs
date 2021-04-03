namespace ApiClient.MastercardConversionRate.Models.Dtos
{
    public record SettlementCurrencyDto
    {
        public CurrencyDto[] Currencies { get; init; }
    }

    public record CurrencyDto
    {
        public string AlphaCode { get; init; }
        public string CurrencyName { get; init; }
    }
}
