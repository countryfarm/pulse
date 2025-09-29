using Marap.Pulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marap.Pulse.Infrastructure.Persistence.Configurations;

public class LocationConfiguration : EntityConfiguration<Location, LocationId, int>
{
  public LocationConfiguration() : base(id => id.Value, value => LocationId.From(value)) { }

  public override void Configure(EntityTypeBuilder<Location> builder)
  {
    base.Configure(builder);

    builder.Property(l => l.Name).IsRequired().HasMaxLength(200);
    builder.Property(l => l.Description).HasMaxLength(500);
  }
}