using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimescaleApi.Application.DTOs;
using TimescaleApi.Application.Services;
using TimescaleApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TimescaleApi.Infrastructure.Services
{
    public class ResultQueryService : IResultQueryService
    {
        private readonly TimescaleDbContext _db;
        public ResultQueryService(TimescaleDbContext db) => _db = db;

        public async Task<IEnumerable<ResultDto>> GetResultsAsync(ResultFilter filter)
        {
            var query = _db.Results.AsQueryable();

            if (!string.IsNullOrEmpty(filter.FileName))
                query = query.Where(r => r.FileName == filter.FileName);
            if (filter.FirstDateFrom.HasValue)
                query = query.Where(r => r.FirstDate >= filter.FirstDateFrom.Value);
            if (filter.FirstDateTo.HasValue)
                query = query.Where(r => r.FirstDate <= filter.FirstDateTo.Value);
            if (filter.AvgValueFrom.HasValue)
                query = query.Where(r => r.AvgValue >= filter.AvgValueFrom.Value);
            if (filter.AvgValueTo.HasValue)
                query = query.Where(r => r.AvgValue <= filter.AvgValueTo.Value);
            if (filter.AvgExecutionTimeFrom.HasValue)
                query = query.Where(r => r.AvgExecutionTime >= filter.AvgExecutionTimeFrom.Value);
            if (filter.AvgExecutionTimeTo.HasValue)
                query = query.Where(r => r.AvgExecutionTime <= filter.AvgExecutionTimeTo.Value);

            return await query
                .Select(r => new ResultDto
                {
                    FileName = r.FileName,
                    FirstDate = r.FirstDate,
                    TimeDeltaSeconds = r.TimeDeltaSeconds,
                    AvgExecutionTime = r.AvgExecutionTime,
                    AvgValue = r.AvgValue,
                    MedianValue = r.MedianValue,
                    MinValue = r.MinValue,
                    MaxValue = r.MaxValue
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ValueDto>> GetLastValuesAsync(string fileName, int count = 10)
        {
            var values = await _db.Values
                .Where(v => v.FileName == fileName)
                .OrderByDescending(v => v.Date)
                .Take(count)
                .OrderBy(v => v.Date)
                .Select(v => new ValueDto
                {
                    Date = v.Date,
                    ExecutionTime = v.ExecutionTime,
                    Value = v.Value
                })
                .ToListAsync();

            return values;
        }
    }
}
