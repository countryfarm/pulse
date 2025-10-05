using FluentAssertions;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.ValueObjects;

public class PurchaseOrderStatusTests
{
  [Fact]
  public void From_KnownValues_ReturnsSingletons()
  {
    PurchaseOrderStatus.From("Draft").Should().Be(PurchaseOrderStatus.Draft);
    PurchaseOrderStatus.From("Submitted").Should().Be(PurchaseOrderStatus.Submitted);
    PurchaseOrderStatus.From("Approved").Should().Be(PurchaseOrderStatus.Approved);
    PurchaseOrderStatus.From("Received").Should().Be(PurchaseOrderStatus.Received);
    PurchaseOrderStatus.From("Cancelled").Should().Be(PurchaseOrderStatus.Cancelled);
  }

  [Fact]
  public void From_Unknown_Throws()
  {
    Action act = () => PurchaseOrderStatus.From("Unknown");

    act.Should().Throw<ArgumentException>().WithMessage("Invalid status: Unknown");
  }

  [Fact]
  public void CanTransitionTo_ValidAndInvalidPaths()
  {
    PurchaseOrderStatus.Draft.CanTransitionTo(PurchaseOrderStatus.Submitted).Should().BeTrue();
    PurchaseOrderStatus.Draft.CanTransitionTo(PurchaseOrderStatus.Cancelled).Should().BeTrue();
    PurchaseOrderStatus.Draft.CanTransitionTo(PurchaseOrderStatus.Approved).Should().BeFalse();

    PurchaseOrderStatus.Submitted.CanTransitionTo(PurchaseOrderStatus.Approved).Should().BeTrue();
    PurchaseOrderStatus.Submitted.CanTransitionTo(PurchaseOrderStatus.Received).Should().BeFalse();

    PurchaseOrderStatus.Approved.CanTransitionTo(PurchaseOrderStatus.Received).Should().BeTrue();

    // Terminal states
    PurchaseOrderStatus.Cancelled.CanTransitionTo(PurchaseOrderStatus.Draft).Should().BeFalse();
    PurchaseOrderStatus.Received.CanTransitionTo(PurchaseOrderStatus.Received).Should().BeTrue();
  }
}
