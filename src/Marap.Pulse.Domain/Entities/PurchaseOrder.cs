using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Entities;

public class PurchaseOrder : Entity<PurchaseOrderId>, IAggregateRoot
{
  private readonly List<PurchaseOrderLine> _lines = new();

  public VendorId VendorId { get; private set; }
  public DateTime OrderDate { get; private set; }
  public PurchaseOrderStatus Status { get; private set; } = null!;
  public IReadOnlyCollection<PurchaseOrderLine> Lines => _lines.AsReadOnly();
  
  private PurchaseOrder() { }

  public PurchaseOrder(VendorId vendorId, DateTime orderDate, PurchaseOrderStatus status)
  {
    VendorId = vendorId;
    OrderDate = orderDate;
    Status = status;
  }

  public void AddLine(PurchaseOrderLine line)
  {
    _lines.Add(line);
  }
  
  public void AddLine(PartId partId, Quantity orderedQuantity)
  {
    var line = new PurchaseOrderLine(partId, orderedQuantity, this.Id);
    _lines.Add(line);
  }
  
  public void TransitionTo(PurchaseOrderStatus nextStatus)
  {
    if (!Status.CanTransitionTo(nextStatus))
      throw new InvalidOperationException($"Cannot transition from {Status} to {nextStatus}");

    Status = nextStatus;
  }
}