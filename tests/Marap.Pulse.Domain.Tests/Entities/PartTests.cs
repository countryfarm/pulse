using FluentAssertions;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.Events;
using Marap.Pulse.Domain.ValueObjects;
using Marap.Pulse.Domain.Tests.Factories;

namespace Marap.Pulse.Domain.Tests.Entities;

public class PartTests
{
  [Fact]
  public void AddStock_ShouldIncreaseTotalQuantity()
  {
    var part = PartFactory.CreateWithId(PartId.From(1), "SKU-001", "MPN-001", "Test Part", Quantity.From(10));
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");    
    var stock = StockItemFactory.CreateWithId(StockItemId.From(1), part.Id, location.Id, Quantity.From(5m), DateTime.UtcNow);

    part.AddStock(stock);

    part.TotalQuantity.Value.Should().Be(5m);
    part.StockItems.Should().ContainSingle();
  }

  [Fact]
  public void IsBelowThreshold_ShouldReturnTrue_WhenStockIsLow()
  {    
    var part = PartFactory.CreateWithId(PartId.From(2), "SKU-002", "MPN-002", "Test Part", Quantity.From(10));
   
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    var stockItem = StockItemFactory.CreateWithId(StockItemId.From(2), part.Id, location.Id, Quantity.From(5m), DateTime.UtcNow);    
    part.AddStock(stockItem);

    part.IsBelowThreshold().Should().BeTrue();
  }

  [Fact]
  public void IsBelowThreshold_ShouldReturnFalse_WhenStockIsSufficient()
  {

    var part = PartFactory.CreateWithId(PartId.From(3), "SKU-003", "MPN-003", "Test Part", Quantity.From(10));

    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    var stockItem = StockItemFactory.CreateWithId(StockItemId.From(3), part.Id, location.Id, Quantity.From(15m), DateTime.UtcNow);    
    part.AddStock(stockItem);

    part.IsBelowThreshold().Should().BeFalse();
  }
  
  [Fact]
  public void Consume_ShouldRaisePartConsumedEvent()
  {
    var part = PartFactory.CreateWithId(PartId.From(4), "SKU-004", "MPN-004", "Test Part", Quantity.From(5));
    
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    var stockItem = StockItemFactory.CreateWithId(StockItemId.From(4), part.Id, location.Id, Quantity.From(10m), DateTime.UtcNow);    
    part.AddStock(stockItem);

    part.Consume(Quantity.From(3m));

    part.Events.Any(e => e is PartConsumed pc && pc.Quantity.Value == 3m).Should().BeTrue();
  }
  
  [Fact]
  public void Consume_ShouldDepleteFirstStockItemBeforeUsingNext()
  {    
    var part = PartFactory.CreateWithId(PartId.From(5), "SKU-005", "MPN-005", "Test Part", Quantity.From(5));
    
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    
    var earlierItem = StockItemFactory.CreateWithId(StockItemId.From(1), part.Id, location.Id, Quantity.From(2m), DateTime.UtcNow.AddDays(-1));  
    part.AddStock(earlierItem);
    
    var laterItem = StockItemFactory.CreateWithId(StockItemId.From(2), part.Id, location.Id, Quantity.From(5m), DateTime.UtcNow);    
    part.AddStock(laterItem);

    part.Consume(Quantity.From(4m));

    part.StockItems.First(s => s.Id == 1).Quantity.Value.Should().Be(0m);
    part.StockItems.First(s => s.Id == 2).Quantity.Value.Should().Be(3m);

    part.Events.Any(e => e is PartConsumed pc && pc.Quantity.Value == 2m).Should().BeTrue();
  }

  [Fact]
  public void Consume_ShouldThrow_WhenNotEnoughStock()
  {
    var part = PartFactory.CreateWithId(PartId.From(6), "SKU-006", "MPN-006", "Test Part", Quantity.From(5));

    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    var stockItem = StockItemFactory.CreateWithId(StockItemId.From(6), part.Id, location.Id, Quantity.From(2m), DateTime.UtcNow);    
    part.AddStock(stockItem);

    Action act = () => part.Consume(Quantity.From(5m));

    act.Should().Throw<InvalidOperationException>()
       .WithMessage("Not enough stock available.");
  }
  
  [Fact]
  public void Consume_ShouldRaiseLowStockDetected_WhenBelowThreshold()
  {
    var part = PartFactory.CreateWithId(PartId.From(7), "SKU-007", "MPN-007", "Test Part", Quantity.From(5));
    
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    var stockItem = StockItemFactory.CreateWithId(StockItemId.From(7), part.Id, location.Id, Quantity.From(6m), DateTime.UtcNow);        
    part.AddStock(stockItem);

    part.Consume(Quantity.From(2m)); // leaves 4, below threshold

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
    var part = PartFactory.CreateWithId(PartId.From(8), "SKU-008", "MPN-008", "Test Part", Quantity.From(5));
    
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    var stockItem = StockItemFactory.CreateWithId(StockItemId.From(8), part.Id, location.Id, Quantity.From(10m), DateTime.UtcNow);  
    part.AddStock(stockItem);

    part.Consume(Quantity.From(3m));
    part.Events.Should().NotBeEmpty();

    part.ClearEvents();
    part.Events.Should().BeEmpty();
  }
}