
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WeatherAPI.Repository.Interface
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<int> CommitAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
