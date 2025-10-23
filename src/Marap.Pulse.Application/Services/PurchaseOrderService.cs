using Marap.Pulse.Application.Dtos;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Marap.Pulse.Application.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
  private readonly IRepository<PurchaseOrder, PurchaseOrderId> _repo;

  public PurchaseOrderService(IRepository<PurchaseOrder, PurchaseOrderId> repo)
  {
    _repo = repo;
  }

  public async Task<int> CreateAsync(CreatePurchaseOrderDto dto, CancellationToken cancellation = default)
  {
    // Convert vendor id (assume Guid underlying) — adjust if Vogen type requires different conversion
  var vendorId = VendorId.From(dto.VendorId);
  var po = new PurchaseOrder(vendorId, dto.OrderDate, Marap.Pulse.Domain.ValueObjects.PurchaseOrderStatus.Draft);
  await _repo.AddAsync(po, cancellation).ConfigureAwait(false);
  await _repo.SaveChangesAsync(cancellation).ConfigureAwait(false);
  return po.Id.Value;
  }

  public async Task<PurchaseOrderDto?> GetAsync(int id, CancellationToken cancellation = default)
  {
    var poId = PurchaseOrderId.From(id);
    var po = await _repo.GetByIdAsync(poId, cancellation).ConfigureAwait(false);
    if (po == null) return null;

  var lines = po.Lines.Select(l =>
  {
    int lineId;
    try
    {
      lineId = l.Id.Value;
    }
    catch
    {
      lineId = 0;
    }
    return new PurchaseOrderLineDto(lineId, l.PartId.Value, l.OrderedQuantity.Value, l.ReceivedQuantity?.Value);
  }).ToList().AsReadOnly();
    var dto = new PurchaseOrderDto(po.Id.Value, po.VendorId.Value, po.OrderDate, po.Status.ToString(), lines);
    return dto;
  }
}
