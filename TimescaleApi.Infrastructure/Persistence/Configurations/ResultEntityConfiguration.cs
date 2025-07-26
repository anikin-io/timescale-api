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
    public class ResultEntityConfiguration : IEntityTypeConfiguration<ResultEntity>
    {
        public void Configure(EntityTypeBuilder<ResultEntity> builder)
        {
            builder.ToTable("results");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.FileName)
                .HasColumnName("file_name")
                .IsRequired();

            builder.Property(x => x.FirstDate)
                .HasColumnName("first_date")
                .IsRequired();

            builder.Property(x => x.TimeDeltaSeconds)
                .HasColumnName("time_delta_seconds")
                .IsRequired();

            builder.Property(x => x.AvgExecutionTime)
                .HasColumnName("avg_execution_time")
                .IsRequired();

            builder.Property(x => x.AvgValue)
                .HasColumnName("avg_value")
                .IsRequired();

            builder.Property(x => x.MedianValue)
                .HasColumnName("median_value")
                .IsRequired();

            builder.Property(x => x.MinValue)
                .HasColumnName("min_value")
                .IsRequired();

            builder.Property(x => x.MaxValue)
                .HasColumnName("max_value")
                .IsRequired();
        }
    }
}
