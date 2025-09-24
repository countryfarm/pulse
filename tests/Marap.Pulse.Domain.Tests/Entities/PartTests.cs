using FluentAssertions;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Entities;

public class PartTests
{
  [Fact]
  public void AddStock_ShouldIncreaseTotalQuantity()
  {
    var part = new Part(1, "SKU-001", "MPN-001", "Test Part", minimumThreshold: 10);
    var stock = new StockItem(1, part.Id, new Quantity(5m), DateTime.UtcNow, locationId: 1);

    part.AddStock(stock);

    part.TotalQuantity.Value.Should().Be(5m);
    part.StockItems.Should().ContainSingle();
  }

  [Fact]
  public void IsBelowThreshold_ShouldReturnTrue_WhenStockIsLow()
  {
    var part = new Part(2, "SKU-002", "MPN-002", "Test Part", minimumThreshold: 10);
    part.AddStock(new StockItem(2, part.Id, new Quantity(5m), DateTime.UtcNow, locationId: 1));

    part.IsBelowThreshold().Should().BeTrue();
  }

  [Fact]
  public void IsBelowThreshold_ShouldReturnFalse_WhenStockIsSufficient()
  {
    var part = new Part(3, "SKU-003", "MPN-003", "Test Part", minimumThreshold: 10);
    part.AddStock(new StockItem(3, part.Id, new Quantity(15m), DateTime.UtcNow, locationId: 1));

    part.IsBelowThreshold().Should().BeFalse();
  }
}