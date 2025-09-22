using Marap.Pulse.Domain.Common;

namespace Marap.Pulse.Domain.Entities;

public class Vendor : Entity<int>
{
    public string Name { get; private set; }
    public int LeadTimeDays { get; private set; }

    public Vendor(int id, string name, int leadTimeDays)
        : base(id)
    {
        Name = name;
        LeadTimeDays = leadTimeDays;
    }
}