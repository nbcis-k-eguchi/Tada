using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tada.Server.FileAccess;

namespace Tada.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(IEnumerable<IFormFile> files)
        {           
            long size = files.Sum(f => f.Length);
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(FileConnection.ActivityReportUploadPath, formFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }   
            return Ok(new { count = files.Count(), size });
        }
    }
}
