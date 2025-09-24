using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Entities;

public class StockItem : Entity<int>
{
  public int PartId { get; private set; }
  public Quantity Quantity { get; private set; }
  public DateTime ReceivedAt { get; private set; }
  public int LocationId { get; private set; }
  public int? PurchaseOrderId { get; private set; }
  public int? VendorId { get; private set; }

  public StockItem(
    int id,
    int partId,
    Quantity quantity,
    DateTime receivedAt,
    int locationId,
    int? purchaseOrderId = null,
    int? vendorId = null)
    : base(id)
  {
    PartId = partId;
    Quantity = quantity;
    ReceivedAt = receivedAt;
    LocationId = locationId;
    PurchaseOrderId = purchaseOrderId;
    VendorId = vendorId;
  }

  public void Consume(Quantity qty)
  {
    if (qty.Value > Quantity.Value)
      throw new InvalidOperationException("Not enough stock available.");

    Quantity = new Quantity(Quantity.Value - qty.Value);
  }
}