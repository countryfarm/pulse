using FluentAssertions;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.ValueObjects;

public class ChangeAmountTests
{
  [Fact]
  public void ChangeAmount_ShouldStoreValue()
  {
    var ca = new ChangeAmount(10m);
    ca.Value.Should().Be(10m);
  }
}