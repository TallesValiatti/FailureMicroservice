using Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Core.Interfaces.Infra.Repositories
{
    public interface IGenerericRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync (Guid id);
        Task<IEnumerable<T>> AddRangeAsync (IEnumerable<T> entities);
    }
}
