using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TelevisionScheduleService.Models;

namespace TelevisionScheduleService
{
    public class TelevisionScheduleService : ITelevisionScheduleService
    {
        private readonly ILogger<TelevisionScheduleService> _logger;

        private readonly IOptions<TelevisionScheduleServiceOptions> _options;

        private readonly IDistributedCache _cache;

        public TelevisionScheduleService(
            ILogger<TelevisionScheduleService> logger,
            IOptions<TelevisionScheduleServiceOptions> options,
            IDistributedCache cache)
        {
            _logger = logger;
            _options = options;
            _cache = cache;
        }

        public async Task<IEnumerable<TelevisionChannel>> FetchAllChannels(CancellationToken cancellationToken = default)
        {
            // Check if channels are in cache
            var channelsFromCache = await _cache.GetValueAsync<TelevisionChannel[]>(CacheConstants.AllChannelsKey, cancellationToken);
            if (channelsFromCache != default)
            {
                _logger.LogInformation("Found channels in cache");
                return channelsFromCache;
            }

            // Fetch channels
            using var client = new HttpClient();
            var response = await client.GetAsync(_options.Value.GetChannelsEndpoint, cancellationToken);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<GetChannelsResponse>(cancellationToken: cancellationToken);
            
            // Fetch logo for all channels
            var channels = new List<TelevisionChannel>();
            foreach (var channel in data.Channels)
            {
                channels.Add(new TelevisionChannel
                {
                    Id = channel.Id,
                    Title = channel.Title,
                    SvgLogo = await FetchSvgOrDefault(channel, client)
                }); ;
            }

            _logger.LogInformation("Fetched channels from api");

            // Cache channels
            await _cache.SetValueAsync(CacheConstants.AllChannelsKey, channels, TimeSpan.FromHours(12), cancellationToken);

            return channels;
        }

        public async Task<IEnumerable<TelevisionProgramDescription>> FetchProgramDescriptions(string channelId, DateTime dateTime, CancellationToken cancellationToken = default)
        {
            // Prepare query
            var query = $"{dateTime.ToString("yyyy-MM-dd")}?ch={channelId}";

            // Fetch program
            var programFromCache = await _cache.GetValueAsync<TelevisionProgramDescription[]>(CacheConstants.ProgramDescriptionPrefix + query, cancellationToken);
            if (programFromCache != default)
            {
                _logger.LogInformation("Found program descriptions in cache");
                return programFromCache;
            }

            // Fetch program descriptions
            using var client = new HttpClient();
            var response = await client.GetAsync(_options.Value.GetProgramSchedulesEndpoint + query, cancellationToken);
            var rawData = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<ProgramSchedule[]>(cancellationToken: cancellationToken);
            _logger.LogInformation("Fetched program descriptions from api");

            // Map and save to cache
            var mapped = data//.ProgramSchedules
                .SelectMany(s => s.ProgramDescriptions)
                .Select(d => new TelevisionProgramDescription
                {
                    Id = d.Id,
                    Title = d.Title,
                    StartTimeUnix = d.StartTimeUnix,
                    StopTimeUnix = d.StopTimeUnix
                });

            // Cache program descriptions
            await _cache.SetValueAsync(CacheConstants.ProgramDescriptionPrefix + query, mapped, TimeSpan.FromHours(2), cancellationToken);

            return mapped;
        }

        private async Task<string> FetchSvgOrDefault(GetChannelsResponseItem channel, HttpClient httpClient)
        {
            try
            {
                // Get svg
                var svg = await httpClient.GetStringAsync(channel.SvgLogoUrl);

                // Sanitize svg
                var sanitized = svg[svg.IndexOf("<svg")..];
                return Regex.Replace(svg, @"[\n\r↵äüöëÿï]+", string.Empty);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to fetch svg for channel: id = '{channel?.Id}', title = '{channel?.Title}'");
                return default;
            }
        }
    }
}
