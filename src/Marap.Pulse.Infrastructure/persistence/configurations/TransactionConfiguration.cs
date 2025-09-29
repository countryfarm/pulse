using Marap.Pulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marap.Pulse.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : EntityConfiguration<Transaction, TransactionId, int>
{
  public TransactionConfiguration() : base(id => id.Value, value => TransactionId.From(value)) { }

  public override void Configure(EntityTypeBuilder<Transaction> builder)
  {
    base.Configure(builder);

    builder.Property(t => t.Timestamp).IsRequired();

    builder.OwnsOne(t => t.ChangeAmount, q =>
    {
      q.Property(x => x.Value).HasColumnName("ChangeAmount").IsRequired();
    });

    builder.Property(t => t.Type)
      .HasConversion<string>()
      .IsRequired();

    // Foreign key conversions
    builder.Property(t => t.PartId)
      .HasConversion(id => id.Value, value => PartId.From(value));

    builder.Property(t => t.LocationId)
      .HasConversion(id => id.Value, value => LocationId.From(value));

    // Relationships
    builder.HasOne<Part>().WithMany().HasForeignKey(t => t.PartId);
    builder.HasOne<Location>().WithMany().HasForeignKey(t => t.LocationId);
  }
}