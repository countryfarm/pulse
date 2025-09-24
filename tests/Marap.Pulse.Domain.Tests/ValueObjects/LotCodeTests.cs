using FluentAssertions;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.ValueObjects;

public class LotCodeTests
{
  [Fact]
  public void LotCode_ShouldStoreValue()
  {
    var lot = new LotCode("LOT123");
    lot.Value.Should().Be("LOT123");
  }
}