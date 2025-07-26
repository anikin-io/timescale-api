using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TimescaleApi.Infrastructure.Persistence.DesignTime
{
    public class TimescaleDbContextFactory : IDesignTimeDbContextFactory<TimescaleDbContext>
    {
        public TimescaleDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TimescaleDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);

            return new TimescaleDbContext(optionsBuilder.Options);
        }
    }
}