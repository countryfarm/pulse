using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Entities;

public class StockItem : Entity<StockItemId>
{
  public PartId PartId { get; private set; }
  public Part Part { get; private set; } = null!;
  public Quantity Quantity { get; private set; }
  public DateTime ReceivedAt { get; private set; }
  public LocationId LocationId { get; private set; }
  public Location Location { get; private set; } = null!;
  public PurchaseOrderId? PurchaseOrderId { get; private set; }
  public VendorId? VendorId { get; private set; }
  
  private StockItem() { }

  public StockItem(
    PartId partId,
    Quantity quantity,
    DateTime receivedAt,
    LocationId locationId,
    PurchaseOrderId? purchaseOrderId = default,
    VendorId? vendorId = default)
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

    Quantity = Quantity.From(Quantity.Value - qty.Value);
  }
}