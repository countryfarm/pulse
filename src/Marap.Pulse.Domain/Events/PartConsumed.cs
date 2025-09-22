using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Events;

public class PartConsumed : DomainEvent
{
    public int PartId { get; }
    public Quantity Quantity { get; }

    public PartConsumed(int partId, Quantity quantity)
    {
        PartId = partId;
        Quantity = quantity;
    }
}