using Devices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Devices.Core.Contracts
{
    public interface IUsageRepository : IBaseRepository<Usage>
    {
        Task AddRangeAsync(IEnumerable<Usage> usages);

        Task<Usage[]> FindByDeviceId(int deviceId);

        Task<bool> AnyBookingOverlappingWithTimespanAsync(int deviceId, DateTime from, DateTime? to);
    }
}
