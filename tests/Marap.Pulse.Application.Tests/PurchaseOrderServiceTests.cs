using FluentAssertions;
using Xunit;
using Marap.Pulse.Application.Dtos;
using Marap.Pulse.TestHelpers;
using Marap.Pulse.Application.Services;
using Marap.Pulse.Application.Tests.Fakes;
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.ValueObjects;
using Marap.Pulse.Domain.Common;
using System.Threading.Tasks;

namespace Marap.Pulse.Application.Tests;

public class PurchaseOrderServiceTests
{
  [Fact]
  public async Task CreateAsync_Should_Create_PurchaseOrder_And_ReturnId()
  {
    // Arrange
    var repo = new InMemoryRepository<PurchaseOrder, PurchaseOrderId>();
    var svc = new PurchaseOrderService(repo);

    var dto = new CreatePurchaseOrderDto(123, System.DateTime.UtcNow);

    // Act
    var id = await svc.CreateAsync(dto);

    // Assert
    id.Should().BeGreaterThan(0);
    var created = await repo.GetByIdAsync(PurchaseOrderId.From(id));
    created.Should().NotBeNull();
    created!.VendorId.Value.Should().Be(123);
  }

  [Fact]
  public async Task GetAsync_Should_Return_MappedDto_When_Exists()
  {
    // Arrange
    var repo = new InMemoryRepository<PurchaseOrder, PurchaseOrderId>();
    var svc = new PurchaseOrderService(repo);

  var po = new PurchaseOrder(VendorId.From(10), System.DateTime.UtcNow, PurchaseOrderStatus.Draft);
  po.AddLine(PartId.From(5), Quantity.From(1.25m));
    await repo.AddAsync(po);

    // Act
    var dto = await svc.GetAsync(po.Id.Value);

    // Assert
    dto.Should().NotBeNull();
  dto!.Id.Should().Be(po.Id.Value);
  dto.Lines.Should().HaveCount(1);
  dto.Lines[0].PartId.Should().Be(5);
  dto.Lines[0].OrderedQuantity.Should().Be(1.25m);
  }

  [Fact]
  public async Task GetAsync_Should_Return_Null_When_NotFound()
  {
    var repo = new InMemoryRepository<PurchaseOrder, PurchaseOrderId>();
    var svc = new PurchaseOrderService(repo);

    var dto = await svc.GetAsync(99999);
    dto.Should().BeNull();
  }

  [Fact]
  public async Task CreateAsync_Persists_VendorId()
  {
    var repo = new InMemoryRepository<PurchaseOrder, PurchaseOrderId>();
    var svc = new PurchaseOrderService(repo);

    var dto = new CreatePurchaseOrderDto(321, System.DateTime.UtcNow);
    var id = await svc.CreateAsync(dto);

    var created = await repo.GetByIdAsync(PurchaseOrderId.From(id));
    created.Should().NotBeNull();
    created!.VendorId.Value.Should().Be(321);
  }

  [Fact]
  public async Task GetAsync_Maps_ReceivedQuantity_When_Present()
  {
    var repo = new InMemoryRepository<PurchaseOrder, PurchaseOrderId>();
    var svc = new PurchaseOrderService(repo);

    // Create PO and assign id
    var po = new PurchaseOrder(VendorId.From(7), System.DateTime.UtcNow, PurchaseOrderStatus.Draft);
    Marap.Pulse.TestHelpers.EntityTestFactory.WithId(po, PurchaseOrderId.From(42));

    // Create line and mark received
    var line = new PurchaseOrderLine(PartId.From(5), Quantity.From(3.5m), po.Id);
    line.MarkReceived(Quantity.From(2.0m));
    po.AddLine(line);

    await repo.AddAsync(po);

    var dto = await svc.GetAsync(po.Id.Value);
    dto.Should().NotBeNull();
    dto!.Lines.Should().HaveCount(1);
    dto.Lines[0].ReceivedQuantity.Should().Be(2.0m);
  }
}
