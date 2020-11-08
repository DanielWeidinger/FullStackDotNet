using Devices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Devices.Core.Contracts
{
    public interface IBaseRepository<T> where T : IEntityObject
    {
        Task<T[]> GetAllAsync(); 

        Task<T> AddAsync(T device);

        Task<T> FindByIdAsync(int id);

        Task<T> Update(T updated);

        T Delete(T entity);
    }
}
