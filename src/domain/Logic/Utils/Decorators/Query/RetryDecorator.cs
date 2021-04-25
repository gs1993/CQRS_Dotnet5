using Logic.Utils.Shared;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Utils.Decorators.Query
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RetryAttribute : Attribute
    {
        public int NumberOfRetries { get; }

        public RetryAttribute(int numberOfRetries)
        {
            NumberOfRetries = numberOfRetries;
        }
    }

    public sealed class RetryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;
        private readonly AsyncRetryPolicy<TResult> _retryPolicy;

        public RetryDecorator(IQueryHandler<TQuery, TResult> handler,
            RetryAttribute retryAttribute)
        {
            _handler = handler ?? throw new ArgumentException(null, nameof(handler));
            var numberOfRetries = retryAttribute?.NumberOfRetries ?? throw new ArgumentException(null, nameof(retryAttribute));
            
            _retryPolicy = Policy<TResult>.Handle<HttpRequestException>().RetryAsync(numberOfRetries);
        }

        public async Task<TResult> Handle(TQuery query)
        {
            return await _retryPolicy.ExecuteAsync(async () => await _handler.Handle(query));
        }
    }
}
