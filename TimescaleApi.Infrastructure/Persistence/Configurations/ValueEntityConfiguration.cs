using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimescaleApi.Domain.Entities;

namespace TimescaleApi.Infrastructure.Persistence.Configurations
{
    public class ValueEntityConfiguration : IEntityTypeConfiguration<ValueEntity>
    {
        public void Configure(EntityTypeBuilder<ValueEntity> builder)
        {
            builder.ToTable("values");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.FileName)
                .HasColumnName("file_name")
                .IsRequired();

            builder.Property(x => x.Date)
                .HasColumnName("date")
                .IsRequired();

            builder.Property(x => x.ExecutionTime)
                .HasColumnName("execution_time")
                .IsRequired();

            builder.Property(x => x.Value)
                .HasColumnName("value")
                .IsRequired();
        }
    }
}
