using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marap.Pulse.Infrastructure.Persistence.Configurations;

public class PurchaseOrderLineConfiguration : EntityConfiguration<PurchaseOrderLine, PurchaseOrderLineId, int>
{
  public PurchaseOrderLineConfiguration() : base(id => id.Value, value => PurchaseOrderLineId.From(value)) { }

  public override void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
  {
    base.Configure(builder);

    builder.Property(o => o.OrderedQuantity)
      .HasVogenConversion(
        vo => vo.Value,
        raw => Quantity.From(raw)
      )
       .HasColumnName("OrderedQuantity")
       .IsRequired();

    builder.Property(r => r.ReceivedQuantity)
      .HasVogenConversion(
        vo => vo.Value,
        raw => Quantity.From(raw)
      )
      .HasColumnName("ReceivedQuantity")
      .IsRequired(false);

    // Foreign key conversions
    builder.Property(l => l.PartId)
      .HasConversion(id => id.Value, value => PartId.From(value));

    builder.Property(l => l.PurchaseOrderId)
      .HasConversion(id => id.Value, value => PurchaseOrderId.From(value));

    // Relationships
    builder.HasOne<PurchaseOrder>().WithMany(po => po.Lines).HasForeignKey(l => l.PurchaseOrderId);
    builder.HasOne<Part>().WithMany().HasForeignKey(l => l.PartId);
  }
}
