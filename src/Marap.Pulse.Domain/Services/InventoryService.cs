using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Services;

public class InventoryService
{
  public void ReceiveStock(
    Part part,
    PurchaseOrderLine line,
    Location location,
    DateTime receivedAt,
    int purchaseOrderId,
    int vendorId)
  {
    var stockItem = new StockItem(
      id: GenerateId(),
      partId: line.PartId,
      quantity: line.OrderedQuantity,
      receivedAt: receivedAt,
      locationId: location.Id,
      purchaseOrderId: purchaseOrderId,
      vendorId: vendorId
    );

    part.AddStock(stockItem);
    line.MarkReceived(line.OrderedQuantity);
  }

  public void ConsumeStock(Part part, Quantity qty)
  {
    var orderedStock = part.StockItems
      .OrderBy(s => s.ReceivedAt)
      .ToList();

    var remaining = qty.Value;

    foreach (var item in orderedStock)
    {
      if (remaining <= 0) break;

      var consumeQty = Math.Min(item.Quantity.Value, remaining);
      item.Consume(new Quantity(consumeQty));
      remaining -= consumeQty;
    }

    if (remaining > 0)
      throw new InvalidOperationException("Not enough stock to consume.");
  }

  private int GenerateId()
  {
    // Replace with your ID generation logic
    return new Random().Next(1000, 9999);
  }
}