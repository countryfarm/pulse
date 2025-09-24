using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Events;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Entities;

public class Part : Entity<int>, IAggregateRoot
{
  private readonly List<StockItem> _stockItems = new();
  private readonly List<DomainEvent> _events = new();
  
  public string Sku { get; private set; }
  public string Mpn { get; private set; }
  public string Description { get; private set; }
  public Quantity MinimumThreshold { get; private set; }
  public Quantity TotalQuantity =>
    new Quantity(_stockItems.Sum(s => s.Quantity.Value));
  
  public IReadOnlyCollection<DomainEvent> Events => _events.AsReadOnly();
  public IReadOnlyCollection<StockItem> StockItems => _stockItems.AsReadOnly();

  public Part(int id, string sku, string mpn, string description, Quantity minimumThreshold)
    : base(id)
  {
    Sku = sku;
    Mpn = mpn;
    Description = description;
    MinimumThreshold = minimumThreshold;
  }

  public void AddStock(StockItem item)
  {
    if (item.PartId != Id)
      throw new InvalidOperationException("Stock item does not belong to this part.");

    _stockItems.Add(item);
  }
  
  public void Consume(Quantity qty)
  {
    foreach (var item in StockItems.OrderBy(s => s.ReceivedAt))
    {
      if (qty.Value == 0) break;

      var toConsume = Math.Min(item.Quantity.Value, qty.Value);
      item.Consume(new Quantity(toConsume));
      qty = new Quantity(qty.Value - toConsume);

      AddEvent(new PartConsumed(Id, new Quantity(toConsume)));
    }

    if (qty.Value > 0)
      throw new InvalidOperationException("Not enough stock available.");

    if (TotalQuantity.Value < MinimumThreshold.Value)
      AddEvent(new LowStockDetected(Id, TotalQuantity));
  }

  public bool IsBelowThreshold()
  {
    return TotalQuantity.Value < MinimumThreshold.Value;
  }
  
  private void AddEvent(DomainEvent evt) => _events.Add(evt);


  public void ClearEvents() => _events.Clear();

}