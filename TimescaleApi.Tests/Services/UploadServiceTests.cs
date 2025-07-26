using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimescaleApi.Infrastructure.Persistence;
using TimescaleApi.Infrastructure.Services;
using TimescaleApi.Tests.Helpers;
using Xunit;

namespace TimescaleApi.Tests.Services
{
    public class UploadServiceTests
    {
        private readonly TimescaleDbContext _db;
        private readonly UploadService _service;

        public UploadServiceTests()
        {
            _db = TestDbContextFactory.Create();
            _service = new UploadService(_db);
        }

        [Fact]
        public async Task ProcessCsvAsync_InvalidCount_ThrowsInvalidOperationException()
        {
            // Arrange: CSV with header only
            var csv = "Date;ExecutionTime;Value\n";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.ProcessCsvAsync("test.csv", stream));
        }

        [Fact]
        public async Task ProcessCsvAsync_NegativeExecutionTime_ThrowsInvalidOperationException()
        {
            // Arrange: 1 record with negative ExecutionTime
            var csv = new StringBuilder();
            csv.AppendLine("Date;ExecutionTime;Value");
            csv.AppendLine("2025-07-25T10:00:00.000Z;-1;10");
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv.ToString()));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.ProcessCsvAsync("test.csv", stream));
        }

        [Fact]
        public async Task ProcessCsvAsync_ValidCsv_SavesValuesAndResult()
        {
            // Arrange: 2 valid records
            var csv = new StringBuilder();
            csv.AppendLine("Date;ExecutionTime;Value");
            csv.AppendLine("2025-07-25T10:00:00.000Z;1.5;5.0");
            csv.AppendLine("2025-07-25T10:00:02.000Z;2.5;15.0");
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv.ToString()));

            // Act
            await _service.ProcessCsvAsync("file.csv", stream);

            // Assert: Values table
            var values = _db.Values.ToList();
            Assert.Equal(2, values.Count);
            Assert.All(values, v => Assert.Equal("file.csv", v.FileName));

            // Assert: Result table
            var result = _db.Results.SingleOrDefault(r => r.FileName == "file.csv");
            Assert.NotNull(result);
            Assert.Equal(2, result.TimeDeltaSeconds); // 2 sec delta
            Assert.Equal((1.5 + 2.5) / 2, result.AvgExecutionTime);
            Assert.Equal((5.0 + 15.0) / 2, result.AvgValue);
            Assert.Equal((5.0 + 15.0) / 2, result.MedianValue);
            Assert.Equal(5.0, result.MinValue);
            Assert.Equal(15.0, result.MaxValue);
        }
    }
}
