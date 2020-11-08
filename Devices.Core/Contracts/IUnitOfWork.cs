using System;
using System.Threading.Tasks;
using Devices.Core.Contracts;

namespace Devices.Core.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUsageRepository UsageRepository { get; set; }
        IDeviceRepository DeviceRepository { get; set; }
        IPersonRepository PersonRepository { get; set; }

        Task<int> SaveChangesAsync();
        Task DeleteDatabaseAsync();
        Task MigrateDatabaseAsync();
        Task CreateDatabaseAsync();
    }
}
