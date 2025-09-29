using Marap.Pulse.Domain.Common;

namespace Marap.Pulse.Domain.Entities;

public class Vendor : Entity<VendorId>
{
    public string Name { get; private set; } = null!;
    public int LeadTimeDays { get; private set; }
    
    private Vendor() { }

    public Vendor(string name, int leadTimeDays)
    {
        Name = name;
        LeadTimeDays = leadTimeDays;
    }
}