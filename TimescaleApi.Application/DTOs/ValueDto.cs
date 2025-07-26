using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimescaleApi.Application.DTOs
{
    public class ValueDto
    {
        public DateTime Date { get; set; }
        public double ExecutionTime { get; set; }
        public double Value { get; set; }
    }
}
