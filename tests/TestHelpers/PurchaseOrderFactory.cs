using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.TestHelpers;

public static class PurchaseOrderFactory
{
  public static PurchaseOrder CreateWithId(PurchaseOrderId id, VendorId vendorId, DateTime orderDate, PurchaseOrderStatus status)
  {
    var po = new PurchaseOrder(vendorId, orderDate, status);
    return EntityTestFactory.WithId(po, id);
  }
}
