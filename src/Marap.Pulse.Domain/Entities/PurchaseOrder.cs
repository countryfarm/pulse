using Marap.Pulse.Domain.Common;

namespace Marap.Pulse.Domain.Entities;

public class PurchaseOrder : Entity<int>, IAggregateRoot
{
  public int VendorId { get; private set; }
  public DateTime OrderDate { get; private set; }
  public string Status { get; private set; }

  private readonly List<PurchaseOrderLine> _lines = new();
  public IReadOnlyCollection<PurchaseOrderLine> Lines => _lines.AsReadOnly();

  public PurchaseOrder(int id, int vendorId, DateTime orderDate, string status)
    : base(id)
  {
    VendorId = vendorId;
    OrderDate = orderDate;
    Status = status;
  }

  public void AddLine(PurchaseOrderLine line)
  {
    _lines.Add(line);
  }
}