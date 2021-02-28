using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WeatherAPI.Repository.Interface
{
    interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task<int> CommitAsync();
    }
}
