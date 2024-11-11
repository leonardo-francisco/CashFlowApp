using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Application.Caching
{
    public class RedisCacheService
    {
        private readonly IDatabase _cache;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _cache = connectionMultiplexer.GetDatabase();
        }

        public async Task SetCacheAsync(string key, string value)
        {
            await _cache.StringSetAsync(key, value);
        }

        public async Task<string> GetCacheAsync(string key)
        {
            return await _cache.StringGetAsync(key);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }
    }
}
