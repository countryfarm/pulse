using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Entities;

public class Transaction : Entity<int>
{
    public int PartId { get; private set; }
    public ChangeAmount ChangeAmount { get; private set; }
    public string Type { get; private set; } // e.g. Receipt, Consume, Adjust
    public DateTime Timestamp { get; private set; }

    public Transaction(int id, int partId, ChangeAmount changeAmount, string type, DateTime timestamp)
        : base(id)
    {
        PartId = partId;
        ChangeAmount = changeAmount;
        Type = type;
        Timestamp = timestamp;
    }
}