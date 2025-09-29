using Marap.Pulse.Domain.Common;

namespace Marap.Pulse.Domain.Entities;

public class Location : Entity<LocationId>
{
  public string Name { get; private set; } = null!;
  public string Type { get; private set; } = null!;
  public string? Description { get; private set; }
  
  private Location() { }

  public Location(string name, string type, string? description = null)
  {
    Name = name;
    Type = type;
    Description = description;
  }
}