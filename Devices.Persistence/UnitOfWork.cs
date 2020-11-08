using Devices.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Devices.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private bool _disposed;

        public IUsageRepository UsageRepository { get; set; }
        public IDeviceRepository DeviceRepository { get; set; }
        public IPersonRepository PersonRepository { get; set; }

        /// <summary>
        /// ConnectionString kommt aus den appsettings.json
        /// </summary>
        public UnitOfWork() : this(new ApplicationDbContext())
        {
        }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            UsageRepository = new UsageRepository(dbContext);
            DeviceRepository = new DeviceRepository(dbContext);
            PersonRepository = new PersonRepository(dbContext);
        }

        public async Task<int> SaveChangesAsync()
        {
            var entities = _dbContext.ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added
                                 || entity.State == EntityState.Modified)
                .Select(e => e.Entity)
                .ToArray();  // Geänderte Entities ermitteln
            
            foreach (var entity in entities)
            {
                if (entity is IValidatableObject)
                {
                    var validationContext = new ValidationContext(entity, null, null);
                    validationContext.InitializeServiceProvider(serviceType => this); //inject UoW for validation

                    Validator.ValidateObject(entity, validationContext, validateAllProperties: true);
                }
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDatabaseAsync() => await _dbContext.Database.EnsureDeletedAsync();
        public async Task MigrateDatabaseAsync() => await _dbContext.Database.MigrateAsync();
        public async Task CreateDatabaseAsync() => await _dbContext.Database.EnsureCreatedAsync();

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    await _dbContext.DisposeAsync();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
