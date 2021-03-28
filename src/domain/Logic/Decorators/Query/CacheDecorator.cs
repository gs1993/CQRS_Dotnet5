using Logic.Models;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using Cache;

namespace Logic.Decorators.Query
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CacheAttribute : Attribute
    {
        public TimeSpan SlidingExpiration { get; }
        public TimeSpan AbsoluteExpiration { get; }

        public CacheAttribute(double slidingExpirationInMinutes, double absoluteExpirationInMinutes)
        {
            SlidingExpiration = TimeSpan.FromMinutes(slidingExpirationInMinutes);
            AbsoluteExpiration = TimeSpan.FromMinutes(absoluteExpirationInMinutes);
        }
    }

    public sealed class CacheDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;
        private readonly ICacheService<TResult> _cacheService;
        private readonly TimeSpan _slidingExpiration;
        private readonly TimeSpan _absoluteExpiration;

        public CacheDecorator(IQueryHandler<TQuery, TResult> handler,
            ICacheService<TResult> cacheService,
            CacheAttribute cacheAttribute)
        {
            _handler = handler ?? throw new ArgumentException(null, nameof(handler));
            _cacheService = cacheService ?? throw new ArgumentException(null, nameof(cacheService));
            _slidingExpiration = cacheAttribute?.SlidingExpiration ?? throw new ArgumentException(null, nameof(cacheAttribute));
            _absoluteExpiration = cacheAttribute?.AbsoluteExpiration ?? throw new ArgumentException(null, nameof(cacheAttribute));
        }


        public async Task<TResult> Handle(TQuery query)
        {
            var key = CreateCacheKey(query);

            var cacheResult = await _cacheService.TryGet(key);
            if (cacheResult != null)
                return cacheResult;

            var handlerResult = await _handler.Handle(query);
            if (handlerResult != null)
                await _cacheService.Create(key, handlerResult, _slidingExpiration, _absoluteExpiration);

            return handlerResult;
        }


        private static string CreateCacheKey(TQuery query)
        {
            return typeof(TQuery) + JsonSerializer.Serialize(query);
        }
    }
}
