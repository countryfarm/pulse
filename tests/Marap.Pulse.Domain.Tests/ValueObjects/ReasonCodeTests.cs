using FluentAssertions;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.ValueObjects;

public class ReasonCodeTests
{
  [Fact]
  public void ReasonCode_ShouldStoreValue()
  {
    var reason = new ReasonCode("Damaged");
    reason.Value.Should().Be("Damaged");
  }
}