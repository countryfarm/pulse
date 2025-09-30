using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Entities;

public class PurchaseOrderLine : Entity<PurchaseOrderLineId>
{
  public PartId PartId { get; private set; }
  public Part Part { get; private set; } = null!;
  public Quantity OrderedQuantity { get; private set; }
  public Quantity? ReceivedQuantity { get; private set; }
  public PurchaseOrderId PurchaseOrderId { get; private set; }
  public PurchaseOrder? PurchaseOrder { get; private set; }
  
  private PurchaseOrderLine() { }

  public PurchaseOrderLine(PartId partId, Quantity orderedQuantity, PurchaseOrderId purchaseOrderId)
  {
      PartId = partId;
      OrderedQuantity = orderedQuantity;
      PurchaseOrderId = purchaseOrderId;
  }
  
  public void MarkReceived(Quantity qty)
  {
    ReceivedQuantity = qty;
  }
}