using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marap.Pulse.Infrastructure.Persistence.Configurations;

public class PartConfiguration : EntityConfiguration<Part, PartId, int>
{
  public PartConfiguration() : base(id => id.Value, value => PartId.From(value)) { }

  public override void Configure(EntityTypeBuilder<Part> builder)
  {
    base.Configure(builder);

    builder.Property(p => p.Sku).IsRequired().HasMaxLength(50);
    builder.Property(p => p.Mpn).HasMaxLength(100);
    builder.Property(p => p.Description).HasMaxLength(500);

    builder.Property(p => p.MinimumThreshold)
      .HasConversion(
        vo => vo.Value,                      // Quantity ? decimal
        raw => Quantity.From(raw)            // decimal ? Quantity
      )
      .HasColumnName("MinimumThreshold")
      .IsRequired();

    builder.Ignore(p => p.TotalQuantity);
    builder.Ignore(p => p.Events);
  }
}