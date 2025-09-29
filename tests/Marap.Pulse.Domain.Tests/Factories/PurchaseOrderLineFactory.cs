using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Factories
{
  public static class PurchaseOrderLineFactory
  {
    public static PurchaseOrderLine CreateWithId(
      PurchaseOrderLineId id,
      PartId partId,
      Quantity orderedQuantity,
      PurchaseOrderId poId)
    {
      var line = new PurchaseOrderLine(partId, orderedQuantity, poId);
      return EntityTestFactory.WithId(line, id);
    }
  }
}