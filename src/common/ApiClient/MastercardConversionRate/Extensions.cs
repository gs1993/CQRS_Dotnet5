using ApiClient.MastercardConversionRate.Config;
using ApiClient.MastercardConversionRate.Interfaces;
using Extensions;
using Mastercard.Developer.OAuth1Signer.Core.Signers;
using Mastercard.Developer.OAuth1Signer.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApiClient.MastercardConversionRate
{
    public static class Extensions
    {
        public static void AddMastercardApi(this IServiceCollection services, MastercardApiConfig rateApiConfig)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            var signingKey = AuthenticationUtils.LoadSigningKey(
                        rateApiConfig.CertPath,
                        rateApiConfig.KeyAlias,
                        rateApiConfig.KeyPassword);

            services.AddSingleton(new SigningKey { Key = signingKey });
            services.AddSingleton(rateApiConfig);
            services.AddScoped<RequestSignerHandler>();

            services.AddRefitClient<ICurrencyRateService>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(rateApiConfig.Url);
                    c.Timeout = TimeSpan.FromSeconds(rateApiConfig.CurrencyRateServiceTimeoutInSeconds);
                }).AddHttpMessageHandler<RequestSignerHandler>();

            services.AddRefitClient<ISettlementCurrenciesService>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(rateApiConfig.Url);
                    c.Timeout = TimeSpan.FromSeconds(rateApiConfig.SettlementCurrenciesServiceTimeoutInSeconds);
                }).AddHttpMessageHandler<RequestSignerHandler>();
        }


        internal class RequestSignerHandler : DelegatingHandler
        {
            private readonly NetHttpClientSigner _signer;

            public RequestSignerHandler(MastercardApiConfig rateApiConfig, SigningKey signingKey)
            {
                _signer = new NetHttpClientSigner(rateApiConfig.ConsumerKey, signingKey.Key);
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                _signer.Sign(request);

                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
