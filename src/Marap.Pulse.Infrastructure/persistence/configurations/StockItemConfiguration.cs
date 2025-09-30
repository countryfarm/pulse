using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Marap.Pulse.Infrastructure.Persistence.Configurations;

public class StockItemConfiguration : EntityConfiguration<StockItem, StockItemId, int>
{
  public StockItemConfiguration() : base(id => id.Value, value => StockItemId.From(value)) { }

  public override void Configure(EntityTypeBuilder<StockItem> builder)
  {
    base.Configure(builder);

    builder.Property(s => s.ReceivedAt).IsRequired();
    
    builder.Property(q => q.Quantity)
      .HasVogenConversion(
        vo => vo.Value,
        raw => Quantity.From(raw)
       )
       .HasColumnName("Quantity")
       .IsRequired();

    // Foreign key conversions
    builder.Property(s => s.PartId)
      .HasConversion(id => id.Value, value => PartId.From(value));

    builder.Property(s => s.LocationId)
      .HasConversion(id => id.Value, value => LocationId.From(value));

    // VendorId?
    builder.Property(s => s.VendorId)
      .HasConversion(new ValueConverter<VendorId?, int?>(
        id => id.HasValue ? id.Value.Value : (int?)null,
        value => value.HasValue ? VendorId.From(value.Value) : (VendorId?)null));

    // PurchaseOrderId?
    builder.Property(s => s.PurchaseOrderId)
      .HasConversion(new ValueConverter<PurchaseOrderId?, int?>(
        id => id.HasValue ? id.Value.Value : (int?)null,
        value => value.HasValue ? PurchaseOrderId.From(value.Value) : (PurchaseOrderId?)null));

    // Relationships
    builder.HasOne<Part>().WithMany(p => p.StockItems).HasForeignKey(s => s.PartId);
    builder.HasOne<Location>().WithMany().HasForeignKey(s => s.LocationId);
    builder.HasOne<Vendor>().WithMany().HasForeignKey(s => s.VendorId);
    builder.HasOne<PurchaseOrder>().WithMany().HasForeignKey(s => s.PurchaseOrderId);
  }
}