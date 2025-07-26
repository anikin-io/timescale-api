using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimescaleApi.API.Models;
using TimescaleApi.Application.Services;

namespace TimescaleApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly IResultQueryService _queryService;
        public ResultsController(IResultQueryService queryService) => _queryService = queryService;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ResultFilterDto dto)
        {
            var filter = new ResultFilter
            {
                FileName = dto.FileName,
                FirstDateFrom = dto.FirstDateFrom,
                FirstDateTo = dto.FirstDateTo,
                AvgValueFrom = dto.AvgValueFrom,
                AvgValueTo = dto.AvgValueTo,
                AvgExecutionTimeFrom = dto.AvgExecutionTimeFrom,
                AvgExecutionTimeTo = dto.AvgExecutionTimeTo
            };
            var results = await _queryService.GetResultsAsync(filter);
            return Ok(results);
        }

        [HttpGet("{fileName}/last10")]
        public async Task<IActionResult> GetLast10(string fileName)
        {
            var values = await _queryService.GetLastValuesAsync(fileName);
            return Ok(values);
        }
    }
}
