using FluentAssertions;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.Events;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Entities;

public class PartTests
{
  [Fact]
  public void AddStock_ShouldIncreaseTotalQuantity()
  {
    var part = new Part(1, "SKU-001", "MPN-001", "Test Part", new Quantity(10));
    var stock = new StockItem(1, part.Id, new Quantity(5m), DateTime.UtcNow, locationId: 1);

    part.AddStock(stock);

    part.TotalQuantity.Value.Should().Be(5m);
    part.StockItems.Should().ContainSingle();
  }

  [Fact]
  public void IsBelowThreshold_ShouldReturnTrue_WhenStockIsLow()
  {
    var part = new Part(2, "SKU-002", "MPN-002", "Test Part", new Quantity(10));
    part.AddStock(new StockItem(2, part.Id, new Quantity(5m), DateTime.UtcNow, locationId: 1));

    part.IsBelowThreshold().Should().BeTrue();
  }

  [Fact]
  public void IsBelowThreshold_ShouldReturnFalse_WhenStockIsSufficient()
  {
    var part = new Part(3, "SKU-003", "MPN-003", "Test Part", new Quantity(10));
    part.AddStock(new StockItem(3, part.Id, new Quantity(15m), DateTime.UtcNow, locationId: 1));

    part.IsBelowThreshold().Should().BeFalse();
  }
  
  [Fact]
  public void Consume_ShouldRaisePartConsumedEvent()
  {
    var part = new Part(1, "SKU-001", "MPN-001", "Test Part", new Quantity(5));
    part.AddStock(new StockItem(1, part.Id, new Quantity(10m), DateTime.UtcNow, locationId: 1));

    part.Consume(new Quantity(3m));

    part.Events.Any(e => e is PartConsumed pc && pc.Quantity.Value == 3m).Should().BeTrue();
  }
  
  [Fact]
  public void Consume_ShouldDepleteFirstStockItemBeforeUsingNext()
  {
    var part = new Part(1, "SKU-001", "MPN-001", "Test Part", new Quantity(5));
    part.AddStock(new StockItem(1, part.Id, new Quantity(2m), DateTime.UtcNow.AddDays(-1), locationId: 1));
    part.AddStock(new StockItem(2, part.Id, new Quantity(5m), DateTime.UtcNow, locationId: 1));

    part.Consume(new Quantity(4m));

    part.StockItems.First(s => s.Id == 1).Quantity.Value.Should().Be(0m);
    part.StockItems.First(s => s.Id == 2).Quantity.Value.Should().Be(3m);

    part.Events.Any(e => e is PartConsumed pc && pc.Quantity.Value == 2m).Should().BeTrue();
  }

  [Fact]
  public void Consume_ShouldThrow_WhenNotEnoughStock()
  {
    var part = new Part(1, "SKU-001", "MPN-001", "Test Part", new Quantity(5));
    part.AddStock(new StockItem(1, part.Id, new Quantity(2m), DateTime.UtcNow, locationId: 1));

    Action act = () => part.Consume(new Quantity(5m));

    act.Should().Throw<InvalidOperationException>()
       .WithMessage("Not enough stock available.");
  }
  
  [Fact]
  public void Consume_ShouldRaiseLowStockDetected_WhenBelowThreshold()
  {
    var part = new Part(1, "SKU-001", "MPN-001", "Test Part", new Quantity(5));
    part.AddStock(new StockItem(1, part.Id, new Quantity(6m), DateTime.UtcNow, locationId: 1));

    part.Consume(new Quantity(2m)); // leaves 4, below threshold

    // Assert PartConsumed event
    part.Events.OfType<PartConsumed>()
        .Should().ContainSingle()
        .Which.Quantity.Value.Should().Be(2m);


    // Assert LowStockDetected event
    part.Events.OfType<LowStockDetected>()
        .Should().ContainSingle()
        .Which.CurrentQuantity.Value.Should().Be(4m);

  }
  
  [Fact]
  public void ClearEvents_ShouldRemoveAllRaisedEvents()
  {
    var part = new Part(1, "SKU-001", "MPN-001", "Test Part", new Quantity(5));
    part.AddStock(new StockItem(1, part.Id, new Quantity(10m), DateTime.UtcNow, locationId: 1));

    part.Consume(new Quantity(3m));
    part.Events.Should().NotBeEmpty();

    part.ClearEvents();
    part.Events.Should().BeEmpty();
  }
}