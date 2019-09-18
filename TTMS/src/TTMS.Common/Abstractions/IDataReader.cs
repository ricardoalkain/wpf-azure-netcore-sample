using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TTMS.Common.Abstractions
{
    public interface IDataReader<TKey, TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(TKey id);
    }
}
