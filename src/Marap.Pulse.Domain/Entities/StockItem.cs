using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Entities;

public class StockItem : Entity<int>
{
    public int PartId { get; private set; }
    public Quantity Quantity { get; private set; }
    public DateTime ReceivedAt { get; private set; }

    public StockItem(int id, int partId, Quantity quantity, DateTime receivedAt)
        : base(id)
    {
        PartId = partId;
        Quantity = quantity;
        ReceivedAt = receivedAt;
    }
}