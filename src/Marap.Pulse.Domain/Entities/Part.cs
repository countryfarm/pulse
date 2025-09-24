using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Entities;

public class Part : Entity<int>, IAggregateRoot
{
  public string Sku { get; private set; }
  public string Mpn { get; private set; }
  public string Description { get; private set; }
  public int MinimumThreshold { get; private set; }

  private readonly List<StockItem> _stockItems = new();
  public IReadOnlyCollection<StockItem> StockItems => _stockItems.AsReadOnly();

  public Part(int id, string sku, string mpn, string description, int minimumThreshold)
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

  public bool IsBelowThreshold()
  {
    var totalQuantity = _stockItems.Sum(s => s.Quantity.Value);
    return totalQuantity < MinimumThreshold;
  }

  public Quantity TotalQuantity =>
    new Quantity(_stockItems.Sum(s => s.Quantity.Value));
}