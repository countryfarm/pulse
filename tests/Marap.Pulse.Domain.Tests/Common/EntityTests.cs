using FluentAssertions;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;
using Marap.Pulse.Domain.Tests.Factories;

namespace Marap.Pulse.Domain.Tests.Common;

public class EntityTests
{
  [Fact]
  public void Entities_WithSameId_AreEqual()
  {
    var id = PartId.From(42);
    var part1 = PartFactory.CreateWithId(id, "SKU-001", "MPN-001", "Test Part", Quantity.From(10));
    var part2 = PartFactory.CreateWithId(id, "SKU-001", "MPN-001", "Test Part", Quantity.From(10));

    Assert.Equal(part1, part2);
    Assert.True(part1 == part2);
  }

  [Fact]
  public void Entities_WithDifferentIds_AreNotEqual()
  {
    var part1 = PartFactory.CreateWithId(PartId.From(1), "SKU-001", "MPN-001", "Test Part", Quantity.From(10));
    var part2 = PartFactory.CreateWithId(PartId.From(2), "SKU-001", "MPN-001", "Test Part", Quantity.From(10));

    Assert.NotEqual(part1, part2);
    Assert.True(part1 != part2);
  }
}