using FluentAssertions;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;
using Marap.Pulse.Domain.Tests.Factories;

namespace Marap.Pulse.Domain.Tests.Entities;

public class StockItemTests
{
  [Fact]
  public void Consume_ShouldReduceQuantity_WhenEnoughStockExists()
  {
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Bin", "Test location");
    var part = PartFactory.CreateWithId(PartId.From(1), "SKU-001", "MPN-001", "Test Part", Quantity.From(10));
    var item = StockItemFactory.CreateWithId(StockItemId.From(1), part.Id, location.Id, Quantity.From(10m), DateTime.UtcNow);
    item.Consume(Quantity.From(4m));

    item.Quantity.Value.Should().Be(6m);
  }

  [Fact]
  public void Consume_ShouldThrow_WhenNotEnoughStock()
  {
    var part = PartFactory.CreateWithId(PartId.From(1), "SKU-001", "MPN-001", "Test Part", Quantity.From(5m));
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    var stockItem = StockItemFactory.CreateWithId(StockItemId.From(2), part.Id, location.Id, Quantity.From(5m), DateTime.UtcNow);

    Action act = () => stockItem.Consume(Quantity.From(10m));

    act.Should().Throw<InvalidOperationException>()
       .WithMessage("Not enough stock available.");
  }
  
  [Fact]
  public void StockItem_ShouldAllowOptionalPurchaseOrder()
  {
    var part = PartFactory.CreateWithId(PartId.From(1), "SKU-001", "MPN-001", "Test Part", Quantity.From(5m));
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");

    var stockItem = StockItemFactory.CreateWithId(
      StockItemId.From(1),
      part.Id,
      location.Id,
      Quantity.From(5m),
      poId: PurchaseOrderId.From(10) // optional
    );

    stockItem.PurchaseOrderId.Should().Be(PurchaseOrderId.From(10));
  }
}