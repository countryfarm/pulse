using Microsoft.Extensions.DependencyInjection;
using Marap.Pulse.Application.Services;

namespace Marap.Pulse.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
    return services;
  }
}
