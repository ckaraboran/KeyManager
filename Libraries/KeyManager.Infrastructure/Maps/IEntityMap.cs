namespace KeyManager.Infrastructure.Maps;

public interface IEntityMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
{

}