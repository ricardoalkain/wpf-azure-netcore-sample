using System.Collections.Generic;
using System.Threading.Tasks;

namespace TTMS.Common.Abstractions
{
    public interface IDataService<TKey, TModel>
    {
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetByIdAsync(TKey key);

        Task<TModel> CreateAsync(TModel model);

        Task UpdateAsync(TModel model);

        Task DeleteAsync(TKey key);
    }
}
