using Marap.Pulse.Domain.Common;

namespace Marap.Pulse.Domain.Entities;

public class PurchaseOrder : Entity<int>, IAggregateRoot
{
    public int VendorId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public string Status { get; private set; }

    public PurchaseOrder(int id, int vendorId, DateTime orderDate, string status)
        : base(id)
    {
        VendorId = vendorId;
        OrderDate = orderDate;
        Status = status;
    }
}