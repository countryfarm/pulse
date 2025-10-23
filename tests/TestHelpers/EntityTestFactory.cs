using System.Reflection;
using Marap.Pulse.Domain.Common;

namespace Marap.Pulse.TestHelpers;

public static class EntityTestFactory
{
  public static TEntity WithId<TEntity, TId>(TEntity entity, TId id)
    where TEntity : Entity<TId>
    where TId : struct
  {
    typeof(TEntity)
      .GetProperty(nameof(Entity<TId>.Id), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
      .SetValue(entity, id);

    return entity;
  }

  public static TEntity WithNullableId<TEntity, TId>(TEntity entity, TId? id)
    where TEntity : Entity<TId>
    where TId : struct
  {
    typeof(TEntity)
      .GetProperty(nameof(Entity<TId>.Id), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
      .SetValue(entity, id);

    return entity;
  }
}
