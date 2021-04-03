using Refit;
using System.Threading.Tasks;

namespace ApiClient.MastercardConversionRate.Interfaces
{
    public interface ICurrencyRateService
    {
        [Get("/conversion-rate")]
        Task<ConversionRateRequest> GetConversionRate(string fxDate, string transCurr, string crdhldBillCurr, decimal transAmt);
    }


    public record ConversionRateRequest
    {
        public Conversion Data { get; init; }
        public string Description { get; init; }
        public string Name { get; init; }
        public string Type { get; init; }

        public bool IsFailure()
        {
            return Type?.ToLower() == "error"
                || Data == null
                || !string.IsNullOrEmpty(Data.ErrorCode)
                || !string.IsNullOrEmpty(Data.ErrorMessage);
        }
    }

    public record Conversion
    {
        public decimal? ConversionRate { get; init; }
        public decimal? CrdhldBillAmt { get; init; }
        public string CrdhldBillCurr { get; init; }
        public string ErrorCode { get; init; }
        public string ErrorMessage { get; init; }
    }
}
