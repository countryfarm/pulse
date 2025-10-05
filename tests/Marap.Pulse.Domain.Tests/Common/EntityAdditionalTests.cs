using FluentAssertions;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Tests.Factories;

namespace Marap.Pulse.Domain.Tests.Common;

public class EntityAdditionalTests
{
  private sealed class TestEntityA : Entity<PartId>
  {
    // Reuse protected ctor
    public TestEntityA() { }
  }

  private sealed class TestEntityB : Entity<PartId>
  {
    public TestEntityB() { }
  }

  [Fact]
  public void Entities_SameIdDifferentRuntimeType_AreNotEqual()
  {
    var id = PartId.From(1);

    var a = new TestEntityA();
    var b = new TestEntityB();

    EntityTestFactory.WithId(a, id);
    EntityTestFactory.WithId(b, id);

    a.Equals(b).Should().BeFalse();
    (a == b).Should().BeFalse();
    a.GetHashCode().Should().NotBe(b.GetHashCode());
  }

  [Fact]
  public void Equality_Operators_HandleNulls()
  {
    Entity<PartId>? left = null;
    Entity<PartId>? right = null;

    (left == right).Should().BeTrue();

    var nonNull = new TestEntityA();
    EntityTestFactory.WithId(nonNull, PartId.From(2));

    (left == nonNull).Should().BeFalse();
    (nonNull != null).Should().BeTrue();
  }
}
