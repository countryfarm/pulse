using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Events;

public class LowStockDetected : DomainEvent
{
    public PartId PartId { get; }
    public Quantity CurrentQuantity { get; }

    public LowStockDetected(PartId partId, Quantity currentQuantity)
    {
        PartId = partId;
        CurrentQuantity = currentQuantity;
    }
}