using FluentAssertions;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Entities;

public class PurchaseOrderTests
{
  [Fact]
  public void AddLine_ShouldIncreaseLines()
  {
    var po = new PurchaseOrder(1, vendorId: 10, DateTime.UtcNow, "Open");
    var line = new PurchaseOrderLine(1, partId: 5, new Quantity(20m));

    po.AddLine(line);

    po.Lines.Should().ContainSingle();
    po.Lines.First().PartId.Should().Be(5);
  }
}