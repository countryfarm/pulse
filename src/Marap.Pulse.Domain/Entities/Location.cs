using Marap.Pulse.Domain.Common;

namespace Marap.Pulse.Domain.Entities;

public class Location : Entity<int>
{
    public string Name { get; private set; }
    public string Type { get; private set; } // e.g. Bin, Cabinet, Lab

    public Location(int id, string name, string type)
        : base(id)
    {
        Name = name;
        Type = type;
    }
}