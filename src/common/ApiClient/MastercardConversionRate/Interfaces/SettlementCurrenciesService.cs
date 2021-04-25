using Refit;
using System.Threading.Tasks;

namespace ApiClient.MastercardConversionRate.Interfaces
{
    public interface ISettlementCurrenciesService
    {
        [Get("/settlement-currencies")]
        Task<SettlementCurrencyRequest> GetSettlementCurrencies();
    }


    public record SettlementCurrencyRequest
    {
        public SettlementCurrency Data { get; init; }
    }

    public record SettlementCurrency
    {
        public Currency[] Currencies { get; init; }
    }

    public record Currency
    {
        public string AlphaCd { get; init; }
        public string CurrNam { get; init; }
    }
}
