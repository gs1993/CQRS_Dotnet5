using Logic.Models;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using Cache;

namespace Logic.Decorators
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class CacheAttribute : Attribute
    {
        public CacheAttribute() { }
    }

    public sealed class CacheDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;
        private readonly ICacheService<TResult> _cacheService;

        public CacheDecorator(IQueryHandler<TQuery, TResult> handler,
            ICacheService<TResult> cacheService)
        {
            _handler = handler ?? throw new ArgumentException(null, nameof(handler));
            _cacheService = cacheService ?? throw new ArgumentException(null, nameof(cacheService));
        }


        public async Task<TResult> Handle(TQuery query)
        {
            var key = CreateCacheKey(query);

            var cacheResult = await _cacheService.TryGet(key);
            if (cacheResult != null)
                return cacheResult;

            var handlerResult = await _handler.Handle(query);
            if (handlerResult != null)
                await _cacheService.Create(key, handlerResult);

            return handlerResult;
        }


        private static string CreateCacheKey(TQuery query)
        {
            return typeof(TQuery) + JsonSerializer.Serialize(query);
        }
    }
}
