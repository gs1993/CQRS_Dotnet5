using ApiClient.MastercardConversionRate.Interfaces;
using ApiClient.MastercardConversionRate.Models.Dtos;
using CSharpFunctionalExtensions;
using Logic.Utils.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Payments.Queries
{
    public sealed class GetSettlementCurrenciesQuery : IQuery<Result<SettlementCurrencyDto>>
    {
    }

    internal sealed class GetSettlementCurrenciesQueryHandler : IQueryHandler<GetSettlementCurrenciesQuery, Result<SettlementCurrencyDto>>
    {
        private readonly ISettlementCurrenciesService _settlementCurrencies;

        public GetSettlementCurrenciesQueryHandler(ISettlementCurrenciesService settlementCurrencies)
        {
            _settlementCurrencies = settlementCurrencies;
        }


        public async Task<Result<SettlementCurrencyDto>> Handle(GetSettlementCurrenciesQuery query)
        {
            var response = await _settlementCurrencies.GetSettlementCurrencies();
            if (response == null)
                return Result.Failure<SettlementCurrencyDto>("API call response");

            var settlementCurrencyDto = new SettlementCurrencyDto
            {
                Currencies = response.Data?.Currencies?
                    .Select(c => new CurrencyDto
                    {
                        AlphaCode = c.AlphaCd,
                        CurrencyName = c.CurrNam
                    })?.ToArray()
                    ?? Array.Empty<CurrencyDto>()
            };

            return Result.Success(settlementCurrencyDto);
        }
    }
}
