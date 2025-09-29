using Microsoft.EntityFrameworkCore;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Infrastructure;

public class PulseDbContext : DbContext
{
  public PulseDbContext(DbContextOptions<PulseDbContext> options) : base(options) { }

  public DbSet<Part> Parts => Set<Part>();
  public DbSet<StockItem> StockItems => Set<StockItem>();
  public DbSet<Vendor> Vendors => Set<Vendor>();
  public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
  public DbSet<Transaction> Transactions => Set<Transaction>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(PulseDbContext).Assembly);
  }

}