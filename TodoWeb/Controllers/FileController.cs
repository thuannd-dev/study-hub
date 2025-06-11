using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TodoWeb.Domains.AppsettingsConfigurations;

namespace TodoWeb.Controllers
{
    public class FileController : Controller
    {
        private readonly FileInformation _fileInformation;
        public FileController(IOptions<FileInformation> fileInformation)
        {
            _fileInformation = fileInformation.Value;
        }

        [HttpGet("{fileName}/read")]
        public async Task<ActionResult> ReadFileAsync(string fileName)
        {
            //ReadAllTextAsync return Task<string>
            //ReadAllLinesAsync return Task<string[]>
            //var content = await System.IO.File.ReadAllTextAsync(path);
            //return Ok(content);
            var path = Path.Combine(_fileInformation.RootDirectory, fileName);
            if (!System.IO.File.Exists(path))
            {
                return NotFound($"File {fileName} not found.");
            }
            using StreamReader reader = new StreamReader(path);
            //khi làm việc với stream phải sử dụng using để đảm bảo dispose - giải phóng tài nguyên
            //tránh memory leak
            string? line = null;
            while ((line = reader.ReadLine()) != null)
            {
                //process line
                Console.WriteLine(line);
            }
            return Ok();
        }

        [HttpPost("{fileName}/write")]
        public async Task<ActionResult> WriteFileAsync(string fileName, string content)
        {
            var path = Path.Combine(_fileInformation.RootDirectory, fileName);
            if (!System.IO.File.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            //await System.IO.File.AppendAllTextAsync(path, content);
            using StreamWriter writer = new StreamWriter(path, append: true);
            //khi làm việc với stream phải sử dụng using để đảm bảo dispose - giải phóng tài nguyên
            await writer.WriteLineAsync(content);
            return Ok();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var path = Path.Combine(_fileInformation.RootDirectory, file.FileName);
            using var stream = new FileStream(path, FileMode.Create);
            try
            {
                await file.CopyToAsync(stream);
                return Ok($"File {file.FileName} uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{fileName}/download")]
        public async Task<ActionResult> DownloadFileAsync(string fileName)
        {
            var path = Path.Combine(_fileInformation.RootDirectory, fileName);
            if (!System.IO.File.Exists(path))
            {
                return NotFound($"File {fileName} not found.");
            }
            try
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
                var contentType = "application/pdf"; // or use a more specific type if known

                Response.Headers["Content-Disposition"] = $"inline; filename=\"{fileName}\"";

                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
