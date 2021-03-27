using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cache.Redis
{
    public class RedisCache<TItem> : ICacheService<TItem> where TItem : class
    {
        private readonly IDistributedCache _redisCache;

        public RedisCache(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }


        public async Task Create(string key, TItem item, TimeSpan slidingExpiration, TimeSpan absoluteExpiration)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("Cache key cannot be empty", nameof(key));
                if (item == null)
                    throw new ArgumentException("Cache value cannot be empty", nameof(item));

                var serialized = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(item));
                if (serialized != null)
                    await _redisCache.SetAsync(key, serialized, new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = slidingExpiration,
                        AbsoluteExpirationRelativeToNow = absoluteExpiration
                    });
            }
            catch (Exception)
            {
                //TODO: Log exception
            }
        }

        public async Task<TItem> TryGet(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("Cache key cannot be empty", nameof(key));

                var cacheResult = await _redisCache.GetAsync(key);
                if (cacheResult == null)
                    return default;

                return JsonSerializer.Deserialize<TItem>(cacheResult);
            }
            catch (Exception)
            {
                //TODO: Log exception
                return default;
            }
        }
    }
}
