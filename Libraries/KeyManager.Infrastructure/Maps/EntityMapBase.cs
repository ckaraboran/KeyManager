using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyManager.Infrastructure.Maps;

public class EntityMapBase<TEntity> : IEntityMap<TEntity> where TEntity : class, IEntityBase
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}