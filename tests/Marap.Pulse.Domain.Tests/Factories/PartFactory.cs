using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;

namespace Marap.Pulse.Domain.Tests.Factories
{
  public static class PartFactory
  {
    public static Part CreateWithId(PartId id, string sku, string mpn, string description, Quantity minThreshold)
    {
      var part = new Part(sku, mpn, description, minThreshold);
      return EntityTestFactory.WithId(part, id);
    }
  }
}