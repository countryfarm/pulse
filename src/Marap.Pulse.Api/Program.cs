using Marap.Pulse.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Marap.Pulse.Application.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Configure DbContext (use same connection from appsettings or environment)
builder.Services.AddDbContext<PulseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PulseDb") ?? Environment.GetEnvironmentVariable("PULSE_DB_CONNECTION")));

// Repository and application service registrations
builder.Services.AddScoped(typeof(Marap.Pulse.Domain.Common.IRepository<,>), typeof(Marap.Pulse.Infrastructure.Repositories.EfRepository<,>));
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
