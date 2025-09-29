using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Events;

public class PartConsumed : DomainEvent
{
    public PartId PartId { get; }
    public Quantity Quantity { get; }

    public PartConsumed(PartId partId, Quantity quantity)
    {
        PartId = partId;
        Quantity = quantity;
    }
}