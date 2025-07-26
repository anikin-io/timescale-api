using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TimescaleApi.Infrastructure.Persistence;

namespace TimescaleApi.Tests.Helpers
{
    internal static class TestDbContextFactory
    {
        public static TimescaleDbContext Create()
        {
            var options = new DbContextOptionsBuilder<TimescaleDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new TimescaleDbContext(options);
        }
    }
}
