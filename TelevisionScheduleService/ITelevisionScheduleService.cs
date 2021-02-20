using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TelevisionScheduleService.Models;

namespace TelevisionScheduleService
{
    public interface ITelevisionScheduleService
    {
        Task<IEnumerable<TelevisionChannel>> FetchAllChannels(CancellationToken cancellationToken = default);

        Task<IEnumerable<TelevisionProgramDescription>> FetchProgramDescriptions(string channelId, DateTime dateTime, CancellationToken cancellationToken = default);
    }
}
