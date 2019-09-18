namespace TTMS.Common.Abstractions
{
    public interface IDataProvider<TKey, TEntity> : IDataReader<TKey, TEntity>, IDataWriter<TKey, TEntity>
    {
    }
}
