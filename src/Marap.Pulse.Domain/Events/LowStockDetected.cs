using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Events;

public class LowStockDetected : DomainEvent
{
    public int PartId { get; }
    public Quantity CurrentQuantity { get; }

    public LowStockDetected(int partId, Quantity currentQuantity)
    {
        PartId = partId;
        CurrentQuantity = currentQuantity;
    }
}