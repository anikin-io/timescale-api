using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimescaleApi.Application.Services;

namespace TimescaleApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService)
            => _uploadService = uploadService;

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не был передан.");

            try
            {
                await _uploadService.ProcessCsvAsync(file.FileName, file.OpenReadStream());
                return Ok("Файл успешно обработан.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера.");
            }
        }
    }
}
