using Extensions;
using Logic.Payments.ApiClient.Config;
using Logic.Payments.ApiClient.Interfaces;
using Mastercard.Developer.OAuth1Signer.Core.Signers;
using Mastercard.Developer.OAuth1Signer.Core.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Payments.ApiClient
{
    public static class Extensions
    {
        public static void AddMastercardApi(this IServiceCollection services, CurrencyRateApiConfig rateApiConfig)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            var signingKey = AuthenticationUtils.LoadSigningKey(
                        @"C:\Users\PLU11555\Downloads\StudentKey-sandbox.p12",
                        "StudentKey",
                        "qwerty123");
            var consumerKey = "zs9j_CASCOzwCb3fYtUhuOA_nvQVhL5Q_bdtVME89263cf44!81a97f08a45147f5b6d0958d589b01230000000000000000";

            var requestSignerHandler = new RequestSignerHandler(consumerKey, signingKey);
            services.AddSingleton(requestSignerHandler);

            services.AddRefitClient<ICurrencyRate>()
                .ConfigureHttpClient(c => 
                {
                    c.BaseAddress = new Uri("https://sandbox.api.mastercard.com/settlement/currencyrate");
                    c.Timeout = TimeSpan.FromSeconds(60);
                }).AddHttpMessageHandler<RequestSignerHandler>();
        }


        public static void UseMastercardApi(this IApplicationBuilder app)
        {
            
        }


        internal class RequestSignerHandler : DelegatingHandler
        {
            private readonly NetHttpClientSigner _signer;

            public RequestSignerHandler(string consumerKey, RSA signingKey)
            {
                _signer = new NetHttpClientSigner(consumerKey, signingKey);
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                _signer.Sign(request);

                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
