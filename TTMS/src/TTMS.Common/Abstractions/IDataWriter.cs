using System.Threading.Tasks;

namespace TTMS.Common.Abstractions
{
    public interface IDataWriter<TKey, TEntity>
    {
        Task<TEntity> CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TKey id);
    }
}
