namespace TimescaleApi.API.Models
{
    public class ResultFilterDto
    {
        public string? FileName { get; set; }
        public DateTime? FirstDateFrom { get; set; }
        public DateTime? FirstDateTo { get; set; }
        public double? AvgValueFrom { get; set; }
        public double? AvgValueTo { get; set; }
        public double? AvgExecutionTimeFrom { get; set; }
        public double? AvgExecutionTimeTo { get; set; }
    }
}
