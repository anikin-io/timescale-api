using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimescaleApi.Application.Services;
using TimescaleApi.Domain.Entities;
using TimescaleApi.Infrastructure.Persistence;
using TimescaleApi.Infrastructure.Services;
using TimescaleApi.Tests.Helpers;
using Xunit;

namespace TimescaleApi.Tests.Services
{
    public class ResultQueryServiceTests
    {
        private readonly TimescaleDbContext _db;
        private readonly ResultQueryService _service;

        public ResultQueryServiceTests()
        {
            _db = TestDbContextFactory.Create();
            _service = new ResultQueryService(_db);

            // data
            var results = new List<ResultEntity>
            {
                new ResultEntity
                {
                    Id = Guid.NewGuid(), FileName = "a.csv", FirstDate = DateTime.UtcNow.AddDays(-1),
                    TimeDeltaSeconds = 10, AvgExecutionTime = 1, AvgValue = 10, MedianValue = 10, MinValue = 5, MaxValue = 15
                },
                new ResultEntity
                {
                    Id = Guid.NewGuid(), FileName = "b.csv", FirstDate = DateTime.UtcNow,
                    TimeDeltaSeconds = 20, AvgExecutionTime = 2, AvgValue = 20, MedianValue = 20, MinValue = 10, MaxValue = 30
                }
            };
            _db.Results.AddRange(results);
            _db.SaveChanges();
        }

        [Fact]
        public async Task GetResultsAsync_NoFilter_ReturnsAll()
        {
            // Act
            var dtos = await _service.GetResultsAsync(new ResultFilter());
            // Assert
            Assert.Equal(2, dtos.Count());
        }

        [Fact]
        public async Task GetResultsAsync_FilterByFileName_ReturnsOne()
        {
            // Act
            var filter = new ResultFilter { FileName = "a.csv" };
            var dtos = await _service.GetResultsAsync(filter);
            // Assert
            Assert.Single(dtos);
            Assert.Equal("a.csv", dtos.First().FileName);
        }

        [Fact]
        public async Task GetLastValuesAsync_ReturnsLatestOrdered()
        {
            // data
            _db.Values.Add(new ValueEntity { Id = Guid.NewGuid(), FileName = "xxx.csv", Date = DateTime.UtcNow.AddMinutes(-2), ExecutionTime = 1, Value = 1 });
            _db.Values.Add(new ValueEntity { Id = Guid.NewGuid(), FileName = "xxx.csv", Date = DateTime.UtcNow.AddMinutes(-1), ExecutionTime = 2, Value = 2 });
            _db.Values.Add(new ValueEntity { Id = Guid.NewGuid(), FileName = "xxx.csv", Date = DateTime.UtcNow, ExecutionTime = 3, Value = 3 });
            _db.SaveChanges();

            // Act
            var values = await _service.GetLastValuesAsync("xxx.csv", count: 2);

            // Assert: should return 2 latest, ordered by date ascending
            Assert.Equal(2, values.Count());
            var list = values.ToList();
            Assert.True(list[0].Date < list[1].Date);
        }
    }
}
