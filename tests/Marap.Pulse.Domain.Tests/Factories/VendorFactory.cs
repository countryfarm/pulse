using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.Common; // where VendorId lives

namespace Marap.Pulse.Domain.Tests.Factories
{
  public static class VendorFactory
  {
    public static Vendor CreateWithId(VendorId id, string name, int leadTime)
    {
        var vendor = new Vendor(name, leadTime);
        return EntityTestFactory.WithId(vendor, id);
    }
  }
}