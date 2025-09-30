using FluentAssertions;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;
using Marap.Pulse.Domain.Tests.Factories;

namespace Marap.Pulse.Domain.Tests.Entities;

public class PurchaseOrderTests
{
  [Fact]
  public void AddLine_ShouldIncreaseLines()
  {
    var vendor = VendorFactory.CreateWithId(VendorId.From(1), "Test Vendor", 5);
    var po = PurchaseOrderFactory.CreateWithId(PurchaseOrderId.From(1), vendor.Id, DateTime.UtcNow, PurchaseOrderStatus.Submitted);
    var part = PartFactory.CreateWithId(PartId.From(5), "SKU-001", "MPN-001", "Test Part", Quantity.From(5m));
    var line = PurchaseOrderLineFactory.CreateWithId(PurchaseOrderLineId.From(1), part.Id, Quantity.From(20m), po.Id);

    po.AddLine(line);

    po.Lines.Should().ContainSingle();
    po.Lines.First().PartId.Value.Should().Be(5);
  }
  
  [Fact]
  public void PurchaseOrder_ShouldAllowValidTransition()
  {
    var vendor = VendorFactory.CreateWithId(VendorId.From(1), "Test Vendor", 5);
    var po = PurchaseOrderFactory.CreateWithId(PurchaseOrderId.From(1), vendor.Id, DateTime.UtcNow, PurchaseOrderStatus.Draft);

    po.TransitionTo(PurchaseOrderStatus.Submitted);
    po.Status.Should().Be(PurchaseOrderStatus.Submitted);

    po.TransitionTo(PurchaseOrderStatus.Approved);
    po.Status.Should().Be(PurchaseOrderStatus.Approved);
  }

  [Fact]
  public void PurchaseOrder_ShouldRejectInvalidTransition()
  {
    var vendor = VendorFactory.CreateWithId(VendorId.From(1), "Test Vendor", 5);
    var po = PurchaseOrderFactory.CreateWithId(PurchaseOrderId.From(1), vendor.Id, DateTime.UtcNow, PurchaseOrderStatus.Draft);

    Action act = () => po.TransitionTo(PurchaseOrderStatus.Received);

    act.Should().Throw<InvalidOperationException>()
       .WithMessage("Cannot transition from Draft to Received");
  }
}