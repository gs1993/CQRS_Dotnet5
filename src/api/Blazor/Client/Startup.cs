using Blazor.Client.Utils;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Blazor.Client
{
    public static class Startup
    {
        public static void RegisterServices(this IServiceCollection services, WebAssemblyHostConfiguration configuration)
        {
            var apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiSettings.Url) });


        }
    }
}
