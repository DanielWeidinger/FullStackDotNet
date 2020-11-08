using Devices.Core.Contracts;
using Devices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Devices.Persistence
{
    public class DeviceRepository : IDeviceRepository
    {
        public DeviceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ApplicationDbContext _dbContext;

        public async Task<Device> AddAsync(Device device) => (await _dbContext.Devices.AddAsync(device)).Entity;

        public async Task<Device> FindByIdAsync(int id) => await _dbContext.Devices.SingleAsync(device => device.Id == id);

        public async Task<Device[]> GetAllAsync() => await _dbContext.Devices.OrderBy(d => d.Name).ToArrayAsync();

        public async Task<Device> Update(Device updated)
        {
            var current = await FindByIdAsync(updated.Id);
            updated.RowVersion = current.RowVersion; //TODO try update schosch
            return (_dbContext.Devices.Update(updated)).Entity;
        }

        public Device Delete(Device entity) => _dbContext.Devices.Remove(entity).Entity;
    }
}
