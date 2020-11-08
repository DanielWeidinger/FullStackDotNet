using Devices.Core.Contracts;
using Devices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices.Persistence
{
    class UsageRepository : IUsageRepository
    {
        private ApplicationDbContext _dbContext;

        public UsageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Usage> AddAsync(Usage usage) => (await _dbContext.Usages.AddAsync(usage)).Entity;

        public Task AddRangeAsync(IEnumerable<Usage> usages) => _dbContext.Usages.AddRangeAsync(usages);

        public Task<bool> AnyBookingOverlappingWithTimespanAsync(int deviceId, DateTime from, DateTime? to) =>_dbContext.Usages.Where(usage => usage.DeviceId == deviceId).AnyAsync(usage => (usage.To == null && (to != null && usage.From <= to) || to == null) || (usage.To != null && usage.From <= from && usage.To >= from));
        

        public Usage Delete(Usage entity) => _dbContext.Usages.Remove(entity).Entity;

        public Task<Usage[]> FindByDeviceId(int deviceId) => _dbContext.Usages.Where(u => u.DeviceId == deviceId)
            .Include(u => u.Person)
            .Include(u => u.Device)
            .ToArrayAsync();

        public Task<Usage> FindByIdAsync(int id) => _dbContext.Usages
            .Include(u => u.Person)
            .Include(u => u.Device)
            .SingleAsync(usage => usage.Id == id);

        public Task<Usage[]> GetAllAsync() => _dbContext.Usages.OrderBy(u => u.Person.LastName)
            .Include(u => u.Person)
            .Include(u => u.Device)
            .ToArrayAsync();

        public async Task<Usage> Update(Usage updated)
        {
            var current = await FindByIdAsync(updated.Id);
            current.PersonId = updated.PersonId;
            current.DeviceId = updated.DeviceId;
            current.From = updated.From;
            current.To = updated.To;
            updated.RowVersion = current.RowVersion;
            return (_dbContext.Usages.Update(current)).Entity;
        }
    }
}
