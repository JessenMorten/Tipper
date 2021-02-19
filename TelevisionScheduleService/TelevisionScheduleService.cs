using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TelevisionScheduleService.Models;

namespace TelevisionScheduleService
{
    public class TelevisionScheduleService : ITelevisionScheduleService
    {
        private readonly ILogger<TelevisionScheduleService> _logger;

        private readonly IDistributedCache _cache;

        public TelevisionScheduleService(ILogger<TelevisionScheduleService> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<TelevisionChannel>> FetchAllChannels(CancellationToken cancellationToken = default)
        {
            var fromCache = await _cache.GetStringAsync("all_channels", cancellationToken);
            if (!string.IsNullOrWhiteSpace(fromCache))
            {
                _logger.LogInformation("Found channels in cache");
                return JsonSerializer.Deserialize<IEnumerable<TelevisionChannel>>(fromCache);
            }

            using var client = new HttpClient();
            var response = await client.GetAsync("...");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<GetChannelsResponse>();
            var channels = new List<TelevisionChannel>();

            foreach (var channel in data.Channels)
            {
                var svg = await client.GetStringAsync(channel.SvgLogoUrl);
                svg = svg
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace("\r\n", string.Empty)
                    .Replace("↵", string.Empty)
                    .Replace("ä", string.Empty);

                var svgStart = svg.IndexOf("<svg");
                svg = svg.Substring(svgStart);

                channels.Add(new TelevisionChannel
                {
                    Id = channel.Id,
                    Title = channel.Title,
                    SvgLogo = svg
                });
            }

            _logger.LogInformation("Fetched channels");

            var json = JsonSerializer.Serialize(channels);
            await _cache.SetStringAsync("all_channels", json, new DistributedCacheEntryOptions
            {
                 AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
            });

            return channels;
        }
    }
}
