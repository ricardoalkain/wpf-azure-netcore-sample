using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TTMS.Data.Entities;

namespace TTMS.Data.Repositories
{
    public interface IRepository<TKey,TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(TKey key);

        Task InsertOrReplaceAsync(TEntity entity);

        Task DeleteAsync(TKey key);
    }
}
