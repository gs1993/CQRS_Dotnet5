using ApiClient.MastercardConversionRate.Models.Dtos;
using Logic.Payments.Queries;
using Logic.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using WebApi.Utils;

namespace WebApi.Controllers
{
    [Route("api/payments")]
    public class PaymentController : BaseController
    {
        private readonly Dispatcher _dispatcher;

        public PaymentController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }


        [HttpGet("conversion-rate")]
        [SwaggerOperation(Summary = "Get conversion rate")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Envelope<ConversionRateDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(Envelope))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(Envelope))]
        public async Task<IActionResult> GetConversionRate(
            [SwaggerParameter(Description = "Transaction amount", Required = true)] decimal transactionAmount,
            [SwaggerParameter(Description = "Transaction currency", Required = true)] string transactionCurrency,
            [SwaggerParameter(Description = "Billing currency", Required = true)] string billingCurrency)
        {
            var response = await _dispatcher.Dispatch(new GetConversionRateQuery(transactionAmount, transactionCurrency, billingCurrency));
            return FromResult(response);
        }

        [HttpGet("settlement-currencies")]
        [SwaggerOperation(Summary = "Get settlement currencies")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Envelope<SettlementCurrencyDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(Envelope))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(Envelope))]
        public async Task<IActionResult> GetSettlementCurrencies()
        {
            var response = await _dispatcher.Dispatch(new GetSettlementCurrenciesQuery());
            return FromResult(response);
        }
    }
}
