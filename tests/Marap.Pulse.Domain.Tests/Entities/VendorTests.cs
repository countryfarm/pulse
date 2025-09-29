using FluentAssertions;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.Tests.Factories;

namespace Marap.Pulse.Domain.Tests.Entities;

public class VendorTests
{
  [Fact]
  public void Vendor_ShouldStoreProperties()
  {
    var vendor = VendorFactory.CreateWithId(VendorId.From(1), "Acme Supplies", 5);

    vendor.Id.Should().Be(VendorId.From(1));
    vendor.Name.Should().Be("Acme Supplies");
    vendor.LeadTimeDays.Should().Be(5);
  }
}