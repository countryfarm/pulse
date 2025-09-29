using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Factories;

public static class TransactionFactory
{
  public static Transaction CreateWithId(
    TransactionId id,
    PartId partId,
    ChangeAmount changeAmount,
    LocationId locationId,
    TransactionType type,
    DateTime timestamp)
  {
    var tx = new Transaction(
      partId,
      changeAmount,
      locationId,
      type,
      timestamp);

    return EntityTestFactory.WithId(tx, id);
  }
}