using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TelevisionScheduleService.Models;

namespace TelevisionScheduleService
{
    public class DummyTelevisionScheduleService : ITelevisionScheduleService
    {
        public async Task<IEnumerable<TelevisionChannel>> FetchAllChannels(CancellationToken cancellationToken = default)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5), cancellationToken);

            return new TelevisionChannel[]
            {
                new TelevisionChannel { Id = "1", Title = "Green Channel", SvgLogo = CreateSvgLogo("#4caf50") },
                new TelevisionChannel { Id = "2", Title = "Red Channel", SvgLogo = CreateSvgLogo("#f44336") },
                new TelevisionChannel { Id = "3", Title = "Blue Channel", SvgLogo = CreateSvgLogo("#2196f3") },
                new TelevisionChannel { Id = "4", Title = "Orange Channel", SvgLogo = CreateSvgLogo("#ff9800") },
                new TelevisionChannel { Id = "5", Title = "Brown Channel", SvgLogo = CreateSvgLogo("#795548") },
                new TelevisionChannel { Id = "6", Title = "Yellow Channel", SvgLogo = CreateSvgLogo("#ffeb3b") },
                new TelevisionChannel { Id = "7", Title = "Pink Channel", SvgLogo = CreateSvgLogo("#e91e63") }
            };
        }

        public async Task<IEnumerable<TelevisionProgramDescription>> FetchProgramDescriptions(string channelId, DateTime dateTime, CancellationToken cancellationToken = default)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5), cancellationToken);

            var startTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
            var stopTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);

            var startTimeUnix = ((DateTimeOffset)startTime).ToUnixTimeSeconds();
            var stopTimeUnix = ((DateTimeOffset)stopTime).ToUnixTimeSeconds();

            return new TelevisionProgramDescription[]
            {
                new TelevisionProgramDescription { Id = "12345", Title = "News and Weather", StartTimeUnix = startTimeUnix, StopTimeUnix = stopTimeUnix }
            };
        }

        private string CreateSvgLogo(string color)
        {
            return "<svg xmlns=\"http://www.w3.org/2000/svg\" " +
            $"width=\"16\" height=\"16\" fill=\"{color}\" " +
            "class=\"bi bi-tv\" viewBox=\"0 0 16 16\"><path d " +
            "= \"M2.5 13.5A.5.5 0 0 1 3 13h10a.5.5 0 0 1 0 1H3" +
            "a.5.5 0 0 1-.5-.5zM13.991 3l.024.001a1.46 1.46 0 " +
            "0 1 .538.143.757.757 0 0 1 .302.254c.067.1.145.27" +
            "7.145.602v5.991l-.001.024a1.464 1.464 0 0 1-.143." +
            "538.758.758 0 0 1-.254.302c-.1.067-.277.145-.602." +
            "145H2.009l-.024-.001a1.464 1.464 0 0 1-.538-.143." +
            "758.758 0 0 1-.302-.254C1.078 10.502 1 10.325 1 1" +
            "0V4.009l.001-.024a1.46 1.46 0 0 1 .143-.538.758.7" +
            "58 0 0 1 .254-.302C1.498 3.078 1.675 3 2 3h11.991" +
            "zM14 2H2C0 2 0 4 0 4v6c0 2 2 2 2 2h12c2 0 2-2 2-2" +
            "V4c0-2-2-2-2-2z\" /> </svg>";
        }
    }
}
