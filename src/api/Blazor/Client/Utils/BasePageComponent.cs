using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace Blazor.Client.Utils
{
    public class BasePageComponent : ComponentBase
    {
        protected HttpClient HttpClient { get; private set; }

        public BasePageComponent(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }


        protected Result<T> Get()
        {
            var response = HttpClient.GetAsync("https://reqres.in/invalid-url");
        }
    }
}
