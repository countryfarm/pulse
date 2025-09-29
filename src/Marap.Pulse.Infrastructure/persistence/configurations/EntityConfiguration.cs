using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marap.Pulse.Infrastructure.Persistence.Configurations;

public abstract class EntityConfiguration<TEntity, TId, TPrimitive> : IEntityTypeConfiguration<TEntity>
  where TEntity : Entity<TId>
  where TId : struct
{
  private readonly Func<TId, TPrimitive> _toPrimitive;
  private readonly Func<TPrimitive, TId> _fromPrimitive;

  protected EntityConfiguration(Func<TId, TPrimitive> toPrimitive, Func<TPrimitive, TId> fromPrimitive)
  {
    _toPrimitive = toPrimitive;
    _fromPrimitive = fromPrimitive;
  }

  public virtual void Configure(EntityTypeBuilder<TEntity> builder)
  {
    builder.HasKey(e => e.Id);

    builder.Property(e => e.Id)
      .ValueGeneratedNever()
      .HasConversion(
        id => _toPrimitive(id),
        value => _fromPrimitive(value));
  }
}