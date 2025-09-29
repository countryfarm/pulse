using Marap.Pulse.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Build configuration (reads appsettings + env vars + user secrets)
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables()
    .Build();

// Set up DI container
var services = new ServiceCollection();
services.AddDbContext<PulseDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("PulseDb") 
        ?? Environment.GetEnvironmentVariable("PULSE_DB_CONNECTION")));

var provider = services.BuildServiceProvider();

// Run migrations
using var scope = provider.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<PulseDbContext>();
Console.WriteLine("Applying migrations...");
db.Database.Migrate();
Console.WriteLine("Done.");