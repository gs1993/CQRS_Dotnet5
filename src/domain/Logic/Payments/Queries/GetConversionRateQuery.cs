using ApiClient.MastercardConversionRate.Interfaces;
using ApiClient.MastercardConversionRate.Models.Dtos;
using CSharpFunctionalExtensions;
using Extensions;
using Logic.Utils.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Payments.Queries
{
    public sealed class GetConversionRateQuery : IQuery<Result<ConversionRateDto>>
    {
        public decimal TransactionAmount { get; }
        public string TransactionCurrency { get; }
        public string BillingCurrency { get; }

        public GetConversionRateQuery(decimal transactionAmount, string transactionCurrency, string billingCurrency)
        {
            TransactionAmount = transactionAmount;
            TransactionCurrency = transactionCurrency;
            BillingCurrency = billingCurrency;
        }
    }

    internal sealed class GetConversionRateQueryHandler : IQueryHandler<GetConversionRateQuery, Result<ConversionRateDto>>
    {
        private readonly ICurrencyRateService _currencyRateClient;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetConversionRateQueryHandler(ICurrencyRateService currencyRateClient, IDateTimeProvider dateTimeProvider)
        {
            _currencyRateClient = currencyRateClient;
            _dateTimeProvider = dateTimeProvider;
        }


        public async Task<Result<ConversionRateDto>> Handle(GetConversionRateQuery query)
        {
            var validationResult = ValdateQuery(query);
            if (validationResult.IsFailure)
                return Result.Failure<ConversionRateDto>(validationResult.Error);

            var fxDate = _dateTimeProvider.UtcNow.ToString("yyyy-MM-dd");
            var response = await _currencyRateClient.GetConversionRate(fxDate, query.TransactionCurrency, query.BillingCurrency, query.TransactionAmount);
            if (response.IsFailure())
                return Result.Failure<ConversionRateDto>(response.Data?.ErrorMessage);

            var conversionRate = new ConversionRateDto
            {
                TransactionAmount = query.TransactionAmount,
                TransactionCurrency = query.TransactionCurrency,
                ConversionRate = response.Data.ConversionRate.GetValueOrDefault(),
                BillingAmount = response.Data.CrdhldBillAmt.GetValueOrDefault(),
                BillingCurrency = response.Data.CrdhldBillCurr
            };
            return Result.Success(conversionRate);
        }


        private static Result ValdateQuery(GetConversionRateQuery query)
        {
            var errors = new List<string>();

            if (query.TransactionAmount <= 0)
                errors.Add("Invalid transaction amount");

            if (string.IsNullOrWhiteSpace(query.BillingCurrency) || query.BillingCurrency.Length != 3)
                errors.Add("Invalid billing currency");

            if (string.IsNullOrWhiteSpace(query.TransactionCurrency) || query.TransactionCurrency.Length != 3)
                errors.Add("Invalid transaction amount");

            if (errors.Any())
                return Result.Failure(string.Join(", ", errors));

            return Result.Success();
        }
    }
}
