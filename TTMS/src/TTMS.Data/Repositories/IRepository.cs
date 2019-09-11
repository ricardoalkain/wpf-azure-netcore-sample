using System.Collections.Generic;
using System.Threading.Tasks;

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
