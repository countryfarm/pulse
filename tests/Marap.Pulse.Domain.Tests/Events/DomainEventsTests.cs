using FluentAssertions;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Events;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Events;

public class DomainEventsTests
{
  [Fact]
  public void PartConsumed_ShouldExposeProperties()
  {
    var evt = new PartConsumed(PartId.From(1), Quantity.From(5m));

    evt.PartId.Should().Be(PartId.From(1));
    evt.Quantity.Value.Should().Be(5m);
  }

  [Fact]
  public void LowStockDetected_ShouldExposeProperties()
  {
    var evt = new LowStockDetected(PartId.From(2), Quantity.From(3m));

    evt.PartId.Should().Be(PartId.From(2));
    evt.CurrentQuantity.Value.Should().Be(3m);
  }
}