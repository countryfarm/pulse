using FluentAssertions;
using Marap.Pulse.Domain.Events;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Events;

public class DomainEventsTests
{
  [Fact]
  public void PartConsumed_ShouldExposeProperties()
  {
    var evt = new PartConsumed(partId: 1, new Quantity(5m));

    evt.PartId.Should().Be(1);
    evt.Quantity.Value.Should().Be(5m);
  }

  [Fact]
  public void LowStockDetected_ShouldExposeProperties()
  {
    var evt = new LowStockDetected(partId: 2, new Quantity(3m));

    evt.PartId.Should().Be(2);
    evt.CurrentQuantity.Value.Should().Be(3m);
  }
}