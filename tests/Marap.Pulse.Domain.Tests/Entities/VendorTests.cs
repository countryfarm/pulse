using FluentAssertions;
using Marap.Pulse.Domain.Entities;

namespace Marap.Pulse.Domain.Tests.Entities;

public class VendorTests
{
  [Fact]
  public void Vendor_ShouldStoreProperties()
  {
    var vendor = new Vendor(1, "Acme Supplies", 5);

    vendor.Id.Should().Be(1);
    vendor.Name.Should().Be("Acme Supplies");
  }
}