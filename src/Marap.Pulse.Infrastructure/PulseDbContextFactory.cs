using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Marap.Pulse.Infrastructure;

public class PulseDbContextFactory : IDesignTimeDbContextFactory<PulseDbContext>
{
    public PulseDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PulseDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=pulse;Username=pulse_user;Password=secret");

        return new PulseDbContext(optionsBuilder.Options);
    }
}