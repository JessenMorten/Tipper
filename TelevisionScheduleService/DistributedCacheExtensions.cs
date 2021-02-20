using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace TelevisionScheduleService
{
    internal static class DistributedCacheExtensions
    {
        internal static async Task SetValueAsync<T>(
            this IDistributedCache cache,
            string key, 
            T item, 
            TimeSpan expiresIn, 
            CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(item);
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiresIn };
            await cache.SetStringAsync(key, json, options, cancellationToken);
        }

        internal static async Task<T> GetValueAsync<T>(
            this IDistributedCache cache,
            string key,
            CancellationToken cancellationToken = default)
        {
            var json = await cache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
