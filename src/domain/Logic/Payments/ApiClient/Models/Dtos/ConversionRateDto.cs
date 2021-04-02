namespace Logic.Payments.ApiClient.Models.Dtos
{
    public record ConversionRateDto
    {
        public decimal TransactionAmount { get; init; }
        public string TransactionCurrency { get; init; }
        public decimal BillingAmount { get; init; }
        public string BillingCurrency { get; init; }
        public decimal ConversionRate { get; init; }
    }
}
