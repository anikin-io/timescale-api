using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimescaleApi.Application.DTOs;

namespace TimescaleApi.Application.Services
{
    public class ResultFilter
    {
        public string? FileName { get; set; }
        public DateTime? FirstDateFrom { get; set; }
        public DateTime? FirstDateTo { get; set; }
        public double? AvgValueFrom { get; set; }
        public double? AvgValueTo { get; set; }
        public double? AvgExecutionTimeFrom { get; set; }
        public double? AvgExecutionTimeTo { get; set; }
    }

    public interface IResultQueryService
    {
        Task<IEnumerable<ResultDto>> GetResultsAsync(ResultFilter filter);
        Task<IEnumerable<ValueDto>> GetLastValuesAsync(string fileName, int count = 10);
    }
}
