namespace Marap.Pulse.Domain.Common;

public abstract class Entity<TId> where TId: struct
{
  public TId Id { get; protected set; } = default;
  
  protected Entity() {}

  protected Entity(TId id)
  {
    Id = id;
  }
}