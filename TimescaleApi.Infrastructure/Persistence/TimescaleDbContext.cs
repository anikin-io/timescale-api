using Microsoft.EntityFrameworkCore;
using TimescaleApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimescaleApi.Infrastructure.Persistence.Configurations;

namespace TimescaleApi.Infrastructure.Persistence
{
    public class TimescaleDbContext : DbContext
    {
        public TimescaleDbContext(DbContextOptions<TimescaleDbContext> options)
            : base(options)
        {
        }

        public DbSet<ValueEntity> Values { get; set; }
        public DbSet<ResultEntity> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ValueEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ResultEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
