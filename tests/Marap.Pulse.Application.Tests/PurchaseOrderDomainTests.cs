using FluentAssertions;
using Marap.Pulse.Application.Tests.Factories;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;
using Marap.Pulse.Domain.Entities;
using Xunit;
using System;

namespace Marap.Pulse.Application.Tests;

public class PurchaseOrderDomainTests
{
  [Fact]
  public void PurchaseOrder_Can_Transition_Status()
  {
    // Arrange
    var po = new PurchaseOrder(VendorId.From(1), DateTime.UtcNow, PurchaseOrderStatus.Draft);
    EntityTestFactory.WithId(po, PurchaseOrderId.From(99));

    // Act
    po.TransitionTo(PurchaseOrderStatus.Submitted);

    // Assert
    po.Status.Should().Be(PurchaseOrderStatus.Submitted);
  }
}
