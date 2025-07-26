using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimescaleApi.Infrastructure.Services
{
    public class CsvRecord
    {
        public DateTime Date { get; set; }
        public double ExecutionTime { get; set; }
        public double Value { get; set; }
    }

}
