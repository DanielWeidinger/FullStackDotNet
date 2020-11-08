using Devices.Core.Contracts;
using Devices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Devices.Persistence
{
    public class PersonRepository: IPersonRepository
    {
        private ApplicationDbContext _dbContext;

        public PersonRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Person> AddAsync(Person usage) => (await _dbContext.People.AddAsync(usage)).Entity;

        public Person Delete(Person entity) => _dbContext.People.Remove(entity).Entity;

        public Task<Person> FindByIdAsync(int id) => _dbContext.People.SingleAsync(person => person.Id == id);

        public Task<Person[]> GetAllAsync() => _dbContext.People.OrderBy(p => p.LastName).ToArrayAsync();

        public async Task<Person> Update(Person updated) {
            var current = await FindByIdAsync(updated.Id);
            current.LastName = updated.LastName;
            current.FirstName = updated.FirstName;
            current.EmailAddress = updated.EmailAddress;
            current.Usages = updated.Usages;
            return (_dbContext.People.Update(current)).Entity;
        }
    }
}
