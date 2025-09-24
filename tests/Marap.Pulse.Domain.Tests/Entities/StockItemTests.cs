using FluentAssertions;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Entities;

public class StockItemTests
{
  [Fact]
  public void Consume_ShouldReduceQuantity_WhenEnoughStockExists()
  {
    var item = new StockItem(1, 1, new Quantity(10m), DateTime.UtcNow, locationId: 1);
    item.Consume(new Quantity(4m));

    item.Quantity.Value.Should().Be(6m);
  }

  [Fact]
  public void Consume_ShouldThrow_WhenNotEnoughStock()
  {
    var item = new StockItem(2, 1, new Quantity(5m), DateTime.UtcNow, locationId: 1);

    Action act = () => item.Consume(new Quantity(10m));

    act.Should().Throw<InvalidOperationException>()
       .WithMessage("Not enough stock available.");
  }
}