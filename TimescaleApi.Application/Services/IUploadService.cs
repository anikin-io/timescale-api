using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimescaleApi.Application.Services
{
    public interface IUploadService
    {
        Task ProcessCsvAsync(string fileName, Stream stream);
    }
}
