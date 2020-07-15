using Api.Core.Entities;
using Api.Core.Interfaces.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Infra.Repositories
{
    public class GenericRepository<T> : IGenerericRepository<T> where T : BaseEntity
    {
        protected DbSet<T> _entities;
        protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException("entity");
            await _entities.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == null) throw new ArgumentNullException("entity");

            T entity = _entities.SingleOrDefault(s => s.Id == id);
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
