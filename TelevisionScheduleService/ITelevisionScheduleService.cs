using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TelevisionScheduleService.Models;

namespace TelevisionScheduleService
{
    public interface ITelevisionScheduleService
    {
        Task<IEnumerable<TelevisionChannel>> FetchAllChannels(CancellationToken cancellationToken = default);
    }
}
