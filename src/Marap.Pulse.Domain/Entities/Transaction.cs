using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Entities;

public class Transaction : Entity<TransactionId>
{
    public PartId PartId { get; private set; }
    public Part Part { get; private set; } = null!;
    public ChangeAmount ChangeAmount { get; private set; } = null!;
    public LocationId LocationId { get; private set; }
    public TransactionType Type { get; private set; }
    public DateTime Timestamp { get; private set; }
    
    private Transaction() { }

    public Transaction(PartId partId, ChangeAmount changeAmount, LocationId locationId, TransactionType type, DateTime timestamp)
    {
        PartId = partId;
        ChangeAmount = changeAmount;
        LocationId = locationId;
        Type = type;
        Timestamp = timestamp;
    }
}