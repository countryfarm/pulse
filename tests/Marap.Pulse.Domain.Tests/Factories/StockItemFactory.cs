using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Factories
{
  public static class StockItemFactory
  {
    public static StockItem CreateWithId(
      StockItemId id,
      PartId partId,
      LocationId locationId,
      Quantity quantity,
      DateTime? receivedAt = null,
      PurchaseOrderId? poId = null,
      VendorId? vendorId = null)
    {
      var stockItem = new StockItem(
        partId,
        quantity,
        receivedAt ?? DateTime.UtcNow,
        locationId,
        poId,
        vendorId
      );

      return EntityTestFactory.WithId(stockItem, id);
    }
  }
}