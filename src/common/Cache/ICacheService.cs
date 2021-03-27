using System;
using System.Threading.Tasks;

namespace Cache
{
    public interface ICacheService<TItem>
    {
        Task<TItem> TryGet(string key);
        Task Create(string key, TItem item, TimeSpan slidingExpiration, TimeSpan absoluteExpiration);
    }
}
