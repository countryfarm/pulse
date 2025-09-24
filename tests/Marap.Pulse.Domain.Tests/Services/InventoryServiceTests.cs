using FluentAssertions;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.Services;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Services;

public class InventoryServiceTests
{
  [Fact]
  public void ReceiveStock_ShouldAddStockItem_AndMarkLineReceived()
  {
    var part = new Part(1, "SKU-001", "MPN-001", "Test Part", minimumThreshold: 5);
    var line = new PurchaseOrderLine(1, part.Id, new Quantity(10m));
    var location = new Location(1, "Main Bin", "Bin");
    var service = new InventoryService();

    service.ReceiveStock(part, line, location, DateTime.UtcNow, purchaseOrderId: 100, vendorId: 200);

    part.StockItems.Should().ContainSingle();
    line.ReceivedQuantity.Should().Be(new Quantity(10m));
    part.TotalQuantity.Value.Should().Be(10m);
  }

  [Fact]
  public void ConsumeStock_ShouldReduceQuantities_InFifoOrder()
  {
    var part = new Part(2, "SKU-002", "MPN-002", "Test Part", minimumThreshold: 5);
    var location = new Location(1, "Main Bin", "Bin");
    var service = new InventoryService();

    // Add two stock items with different ReceivedAt
    var older = new StockItem(1, part.Id, new Quantity(5m), DateTime.UtcNow.AddDays(-2), location.Id);
    var newer = new StockItem(2, part.Id, new Quantity(10m), DateTime.UtcNow, location.Id);
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
    var part = new Part(3, "SKU-003", "MPN-003", "Test Part", minimumThreshold: 5);
    var location = new Location(1, "Main Bin", "Bin");
    var service = new InventoryService();

    part.AddStock(new StockItem(1, part.Id, new Quantity(3m), DateTime.UtcNow, location.Id));

    Action act = () => service.ConsumeStock(part, new Quantity(10m));

    act.Should().Throw<InvalidOperationException>()
       .WithMessage("Not enough stock to consume.");
  }
}