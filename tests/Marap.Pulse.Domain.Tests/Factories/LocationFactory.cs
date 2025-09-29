using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;

namespace Marap.Pulse.Domain.Tests.Factories
{
  public static class LocationFactory
  {
    public static Location CreateWithId(LocationId id, string name, string type, string? description = null)
    {
      var location = new Location(name, type, description);
      return EntityTestFactory.WithId(location, id);
    }
  }
}