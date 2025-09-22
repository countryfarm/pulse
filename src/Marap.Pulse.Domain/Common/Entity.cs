namespace Marap.Pulse.Domain.Common;

public abstract class Entity<TId>
{
  public TId Id { get; protected set; }

  protected Entity(TId id)
  {
    Id = id;
  }
}