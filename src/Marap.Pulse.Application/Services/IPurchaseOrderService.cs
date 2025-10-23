using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Application.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Marap.Pulse.Application.Services;

public interface IPurchaseOrderService
{
  Task<int> CreateAsync(CreatePurchaseOrderDto dto, CancellationToken cancellation = default);
  Task<PurchaseOrderDto?> GetAsync(int id, CancellationToken cancellation = default);
}
