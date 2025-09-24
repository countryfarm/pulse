using FluentAssertions;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.ValueObjects;

public class QuantityTests
{
  [Fact]
  public void Constructor_ShouldThrow_WhenNegative()
  {
    Action act = () => new Quantity(-1m);

    act.Should().Throw<ArgumentException>()
       .WithMessage("Quantity cannot be negative.");
  }

  [Fact]
  public void Value_ShouldReturnDecimal()
  {
    var qty = new Quantity(2.5m);

    qty.Value.Should().Be(2.5m);
  }

  [Fact]
  public void AdditionOperator_ShouldReturnCorrectSum()
  {
    var q1 = new Quantity(2m);
    var q2 = new Quantity(3.5m);

    var result = q1 + q2;

    result.Value.Should().Be(5.5m);
  }
}