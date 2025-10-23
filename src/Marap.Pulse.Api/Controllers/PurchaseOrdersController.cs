using Microsoft.AspNetCore.Mvc;
using Marap.Pulse.Application.Services;
using Marap.Pulse.Application.Dtos;

namespace Marap.Pulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrdersController : ControllerBase
{
  private readonly IPurchaseOrderService _svc;

  public PurchaseOrdersController(IPurchaseOrderService svc)
  {
    _svc = svc;
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> Get(int id)
  {
    var po = await _svc.GetAsync(id);
    if (po == null) return NotFound();
    return Ok(po);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreatePurchaseOrderDto dto)
  {
    var id = await _svc.CreateAsync(dto);
    return CreatedAtAction(nameof(Get), new { id }, null);
  }
}
