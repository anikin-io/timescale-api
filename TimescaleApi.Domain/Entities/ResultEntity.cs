using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimescaleApi.Domain.Entities
{
    public class ResultEntity
    {
        public Guid Id { get; set; }     
        public string FileName { get; set; } = null!;
        public DateTime FirstDate { get; set; }
        public double TimeDeltaSeconds { get; set; }
        public double AvgExecutionTime { get; set; }
        public double AvgValue { get; set; }
        public double MedianValue { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
    }
}
