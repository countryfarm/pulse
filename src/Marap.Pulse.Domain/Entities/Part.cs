using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Entities;

public class Part : Entity<int>, IAggregateRoot
{
    public string Sku { get; private set; }
    public string Mpn { get; private set; }
    public string Description { get; private set; }
    public Quantity MinimumThreshold { get; private set; }

    public Part(int id, string sku, string mpn, string description, Quantity minimumThreshold)
        : base(id)
    {
        Sku = sku;
        Mpn = mpn;
        Description = description;
        MinimumThreshold = minimumThreshold;
    }
}