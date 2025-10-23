using Marap.Pulse.TestHelpers;

namespace Marap.Pulse.Application.Tests.Factories;

public static class EntityTestFactory
{
  public static TEntity WithId<TEntity, TId>(TEntity entity, TId id)
    where TEntity : Marap.Pulse.Domain.Common.Entity<TId>
    where TId : struct
    => Marap.Pulse.TestHelpers.EntityTestFactory.WithId(entity, id);

  public static TEntity WithNullableId<TEntity, TId>(TEntity entity, TId? id)
    where TEntity : Marap.Pulse.Domain.Common.Entity<TId>
    where TId : struct
    => Marap.Pulse.TestHelpers.EntityTestFactory.WithNullableId(entity, id);
}
