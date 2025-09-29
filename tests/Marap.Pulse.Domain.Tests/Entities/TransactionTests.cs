using FluentAssertions;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;
using Marap.Pulse.Domain.Tests.Factories;

namespace Marap.Pulse.Domain.Tests.Entities;

public class TransactionTests
{
  [Fact]
  public void Transaction_ShouldStoreProperties()
  {
    var part = PartFactory.CreateWithId(PartId.From(2), "SKU-001", "MPN-001", "Test Part", new Quantity(5m));
    var location = LocationFactory.CreateWithId(LocationId.From(1), "Main Bin", "Bin");
    var tx = TransactionFactory.CreateWithId(TransactionId.From(1), part.Id, new ChangeAmount(5m), location.Id, TransactionType.Receipt, DateTime.UtcNow);

    tx.PartId.Should().Be(PartId.From(2));
    tx.ChangeAmount.Value.Should().Be(5m);
    tx.Type.Should().Be(TransactionType.Receipt);
  }
}