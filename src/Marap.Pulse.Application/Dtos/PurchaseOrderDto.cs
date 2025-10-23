using System;
using System.Collections.Generic;

namespace Marap.Pulse.Application.Dtos;

public record PurchaseOrderLineDto(int Id, int PartId, decimal OrderedQuantity, decimal? ReceivedQuantity);

public record PurchaseOrderDto(int Id, int VendorId, DateTime OrderDate, string Status, IReadOnlyList<PurchaseOrderLineDto> Lines);

public record CreatePurchaseOrderDto(int VendorId, DateTime OrderDate);
