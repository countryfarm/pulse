using FluentAssertions;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.Tests.Factories;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Entities;

public class PurchaseOrderLineTests
{
  [Fact]
  public void Constructor_SetsProperties()
  {
    var partId = PartId.From(10);
    var qty = Quantity.From(15m);
    var poId = PurchaseOrderId.From(5);

    var line = PurchaseOrderLineFactory.CreateWithId(PurchaseOrderLineId.From(1), partId, qty, poId);

    line.PartId.Should().Be(partId);
    line.OrderedQuantity.Should().Be(qty);
    line.PurchaseOrderId.Should().Be(poId);
    line.ReceivedQuantity.Should().BeNull();
  }

  [Fact]
  public void MarkReceived_SetsReceivedQuantity()
  {
    var partId = PartId.From(11);
    var qty = Quantity.From(7m);
    var poId = PurchaseOrderId.From(6);

    var line = PurchaseOrderLineFactory.CreateWithId(PurchaseOrderLineId.From(2), partId, qty, poId);

    line.MarkReceived(Quantity.From(3m));

    line.ReceivedQuantity.Should().NotBeNull();
    line.ReceivedQuantity.Should().Be(Quantity.From(3m));
  }
}
