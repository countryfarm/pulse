using Marap.Pulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marap.Pulse.Infrastructure.Persistence.Configurations;

public class PurchaseOrderLineConfiguration : EntityConfiguration<PurchaseOrderLine, PurchaseOrderLineId, int>
{
  public PurchaseOrderLineConfiguration() : base(id => id.Value, value => PurchaseOrderLineId.From(value)) { }

  public override void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
  {
    base.Configure(builder);

    builder.OwnsOne(l => l.OrderedQuantity, q =>
    {
      q.Property(x => x.Value).HasColumnName("OrderedQuantity").IsRequired();
    });

    builder.OwnsOne(l => l.ReceivedQuantity, q =>
    {
      q.Property(x => x.Value).HasColumnName("ReceivedQuantity");
    });

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