using FluentAssertions;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.Services;
using Marap.Pulse.Domain.ValueObjects;
using Marap.Pulse.Domain.Tests.Factories;

namespace Marap.Pulse.Domain.Tests.Services;

public class InventoryServiceTests
{
  [Fact]
  public void ReceiveStock_ShouldAddStockItem_AndMarkLineReceived()
  {
    var part = PartFactory.CreateWithId(PartId.From(1), "SKU-001", "MPN-001", "Test Part", new Quantity(5m));
    var vendor = VendorFactory.CreateWithId(VendorId.From(1), "Test Vendor", 5);
    var po = PurchaseOrderFactory.CreateWithId(PurchaseOrderId.From(1), vendor.Id, DateTime.UtcNow, PurchaseOrderStatus.Received);
    var line = PurchaseOrderLineFactory.CreateWithId(PurchaseOrderLineId.From(1), part.Id, new Quantity(10m), po.Id);
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    var service = new InventoryService();

    service.ReceiveStock(part, line, location, DateTime.UtcNow, po.Id, vendor.Id);

    part.StockItems.Should().ContainSingle();
    line.ReceivedQuantity.Should().Be(new Quantity(10m));
    part.TotalQuantity.Value.Should().Be(10m);
  }

  [Fact]
  public void ConsumeStock_ShouldReduceQuantities_InFifoOrder()
  {
    var part = PartFactory.CreateWithId(PartId.From(2), "SKU-002", "MPN-002", "Test Part", new Quantity(5m));
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    var service = new InventoryService();

    // Add two stock items with different ReceivedAt
    var older = StockItemFactory.CreateWithId(StockItemId.From(1), part.Id, location.Id, new Quantity(5m), DateTime.UtcNow.AddDays(-2));
    var newer = StockItemFactory.CreateWithId(StockItemId.From(2), part.Id, location.Id, new Quantity(10m), DateTime.UtcNow);
    part.AddStock(older);
    part.AddStock(newer);

    service.ConsumeStock(part, new Quantity(8m));

    older.Quantity.Value.Should().Be(0m);   // consumed first
    newer.Quantity.Value.Should().Be(7m);   // consumed remainder
    part.TotalQuantity.Value.Should().Be(7m);
  }

  [Fact]
  public void ConsumeStock_ShouldThrow_WhenNotEnoughStock()
  {
    var part = PartFactory.CreateWithId(PartId.From(3), "SKU-003", "MPN-003", "Test Part", new Quantity(5m));
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    var service = new InventoryService();

    var stockItem = StockItemFactory.CreateWithId(StockItemId.From(1), part.Id, location.Id, new Quantity(3m), DateTime.UtcNow);
    part.AddStock(stockItem);

    Action act = () => service.ConsumeStock(part, new Quantity(10m));

    act.Should().Throw<InvalidOperationException>()
       .WithMessage("Not enough stock to consume.");
  }
}