using Marap.Pulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marap.Pulse.Infrastructure.Persistence.Configurations;

public class VendorConfiguration : EntityConfiguration<Vendor, VendorId, int>
{
  public VendorConfiguration() : base(id => id.Value, value => VendorId.From(value)) { }

  public override void Configure(EntityTypeBuilder<Vendor> builder)
  {
    base.Configure(builder);

    builder.Property(v => v.Name).IsRequired().HasMaxLength(200);
    builder.Property(v => v.LeadTimeDays).IsRequired();
  }
}