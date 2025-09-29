using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marap.Pulse.Infrastructure.Persistence.Configurations;

public class PurchaseOrderConfiguration : EntityConfiguration<PurchaseOrder, PurchaseOrderId, int>
{
  public PurchaseOrderConfiguration() : base(id => id.Value, value => PurchaseOrderId.From(value)) { }

  public override void Configure(EntityTypeBuilder<PurchaseOrder> builder)
  {
    base.Configure(builder);

    builder.Property(po => po.OrderDate).IsRequired();

    builder.OwnsOne(po => po.Status, status =>
    {
      status.Property(s => s.Value)
        .HasColumnName("Status")
        .IsRequired();
    });

    // Foreign key conversion
    builder.Property(po => po.VendorId)
      .HasConversion(id => id.Value, value => VendorId.From(value));

    builder.HasOne<Vendor>().WithMany().HasForeignKey(po => po.VendorId);
  }
}