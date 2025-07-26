using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimescaleApi.Domain.Entities
{
    public class ValueEntity
    {
        public Guid Id { get; set; }   
        public string FileName { get; set; } = null!;
        public DateTime Date { get; set; }
        public double ExecutionTime { get; set; }
        public double Value { get; set; }
    }
}
