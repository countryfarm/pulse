using FluentAssertions;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Entities;

public class TransactionTests
{
  [Fact]
  public void Transaction_ShouldStoreProperties()
  {
    var tx = new Transaction(1, 2, new ChangeAmount(5m), 3, "Receipt", DateTime.UtcNow);

    tx.PartId.Should().Be(2);
    tx.ChangeAmount.Value.Should().Be(5m);
    tx.Type.Should().Be("Receipt");
  }
}