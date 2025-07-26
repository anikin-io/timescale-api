using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimescaleApi.Application.Services;
using TimescaleApi.Domain.Entities;
using TimescaleApi.Infrastructure.Persistence;

namespace TimescaleApi.Infrastructure.Services
{
    public class UploadService : IUploadService
    {
        private readonly TimescaleDbContext _db;

        public UploadService(TimescaleDbContext db)
        {
            _db = db;
        }

        public async Task ProcessCsvAsync(string fileName, Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                MissingFieldFound = null,
                BadDataFound = null
            });

            var records = csv.GetRecords<CsvRecord>().ToList();
            if (records.Count < 1 || records.Count > 10_000)
                throw new InvalidOperationException("Неверное количество строк (должно быть от 1 до 10 000).");

            var values = new List<ValueEntity>(records.Count);
            foreach (var rec in records)
            {
                if (rec.Date < new DateTime(2000, 1, 1) || rec.Date > DateTime.UtcNow)
                    throw new InvalidOperationException("Date выходит за допустимый диапазон.");

                if (rec.ExecutionTime < 0)
                    throw new InvalidOperationException("ExecutionTime не может быть меньше 0.");

                if (rec.Value < 0)
                    throw new InvalidOperationException("Value не может быть меньше 0.");

                values.Add(new ValueEntity
                {
                    Id = Guid.NewGuid(),
                    FileName = fileName,
                    Date = rec.Date.ToUniversalTime(),
                    ExecutionTime = rec.ExecutionTime,
                    Value = rec.Value
                });
            }

            await using var tx = await _db.Database.BeginTransactionAsync();
            var oldValues = _db.Values.Where(v => v.FileName == fileName);
            _db.Values.RemoveRange(oldValues);

            await _db.Values.AddRangeAsync(values);
            await _db.SaveChangesAsync();

            var dates = values.Select(v => v.Date).OrderBy(d => d).ToList();
            var execTimes = values.Select(v => v.ExecutionTime).ToList();
            var vals = values.Select(v => v.Value).OrderBy(v => v).ToList();

            var firstDate = dates.First();
            var lastDate = dates.Last();
            var deltaSeconds = (lastDate - firstDate).TotalSeconds;
            var avgExec = execTimes.Average();
            var avgVal = vals.Average();
            var medianVal = vals.Count % 2 == 1
                ? vals[vals.Count / 2]
                : (vals[(vals.Count / 2) - 1] + vals[vals.Count / 2]) / 2.0;
            var minVal = vals.First();
            var maxVal = vals.Last();

            var oldResult = _db.Results.SingleOrDefault(r => r.FileName == fileName);
            if (oldResult != null)
                _db.Results.Remove(oldResult);

            var resultEntity = new ResultEntity
            {
                Id = Guid.NewGuid(),
                FileName = fileName,
                FirstDate = firstDate,
                TimeDeltaSeconds = deltaSeconds,
                AvgExecutionTime = avgExec,
                AvgValue = avgVal,
                MedianValue = medianVal,
                MinValue = minVal,
                MaxValue = maxVal
            };
            await _db.Results.AddAsync(resultEntity);
            await _db.SaveChangesAsync();

            await tx.CommitAsync();
        }
    }
}
