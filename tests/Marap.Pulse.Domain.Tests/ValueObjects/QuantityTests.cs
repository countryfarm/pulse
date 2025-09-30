using FluentAssertions;
using Vogen;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.ValueObjects;

public class QuantityTests
{
  [Fact]
  public void Constructor_ShouldThrow_WhenNegative()
  {
    Action act = () => Quantity.From(-1m);

    act.Should().Throw<ValueObjectValidationException>()
       .WithMessage("Quantity cannot be negative.");
  }

  [Fact]
  public void Value_ShouldReturnDecimal()
  {
    var qty = Quantity.From(2.5m);

    qty.Value.Should().Be(2.5m);
  }

  [Fact]
  public void AdditionOperator_ShouldReturnCorrectSum()
  {
    var q1 = Quantity.From(2m);
    var q2 = Quantity.From(3.5m);

    var result = q1 + q2;

    result.Value.Should().Be(5.5m);
  }
  
  [Fact]
  public void Quantity_Addition_Works()
  {
    var q1 = Quantity.From(5);
    var q2 = Quantity.From(3);

    var result = q1 + q2;

    Assert.Equal(Quantity.From(8), result);
  }
  
  [Fact]
  public void Quantity_Comparison_Works()
  {
    var low = Quantity.From(2);
    var high = Quantity.From(5);

    Assert.True(low < high);
    Assert.True(high > low);
  }
}