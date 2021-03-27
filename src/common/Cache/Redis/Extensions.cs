using Microsoft.Extensions.DependencyInjection;

namespace Cache.Redis
{
    public static class Extensions
    {
        public static void RegisterRedis(this IServiceCollection services, RedisSettings redisSettings)
        {
            services.AddDistributedRedisCache(options =>
            {
                string server = redisSettings.Server;
                string port = redisSettings.Port;
                options.Configuration = $"{server}:{port}";
                //options.InstanceName = redisSettings.InstanceName;
            });
        }
    }
}
