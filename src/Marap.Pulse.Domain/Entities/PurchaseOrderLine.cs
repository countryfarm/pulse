using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Entities;

public class PurchaseOrderLine : Entity<int>
{
  public int PartId { get; private set; }
  public Quantity OrderedQuantity { get; private set; }
  public Quantity? ReceivedQuantity { get; private set; }

  public PurchaseOrderLine(int id, int partId, Quantity orderedQuantity)
    : base(id)
  {
    PartId = partId;
    OrderedQuantity = orderedQuantity;
  }

  public void MarkReceived(Quantity qty)
  {
    ReceivedQuantity = qty;
  }
}