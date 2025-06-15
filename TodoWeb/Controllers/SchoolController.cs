using System.Xml;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using RazorLight;
using TodoWeb.Application.ActionFilters;
using TodoWeb.Application.BackgroundJobs;
using TodoWeb.Application.Dtos.SchoolModel;
using TodoWeb.Application.Services.School;
using TodoWeb.Constants.Enums;
using TodoWeb.Domains.AppsettingsConfigurations;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[TypeFilter(typeof(AuthorizationFilter), Arguments = [new Role[] { Role.Admin, Role.User }])]
    [Authorize(Roles = "User, Admin")]//atribute này ko phục vụ cho session
    public class SchoolController : Controller
    {
        private readonly ISchoolService _schoolService;
        private readonly FileInformation _fileInformation;
        public SchoolController(ISchoolService schoolService, IOptions<FileInformation> fileInformation)
        {
            _schoolService = schoolService;
            _fileInformation = fileInformation.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchools(int? schoolId)
        {

            return Ok(await _schoolService.GetSchools(schoolId));
        }

        [HttpGet("{id}/detail")]
        public SchoolStudentViewModel GetSchoolsDetails(int id)
        {
            return _schoolService.GetSchoolDetail(id);
        }

        [HttpPost]
        public int Post(SchoolViewModel school)
        {
            return _schoolService.Post(school);
        }
        [HttpPut]
        public int Put(SchoolViewModel school)
        {
            return _schoolService.Put(school);
        }

        [HttpDelete]
        public int Delete(int schoolID)
        {
            return _schoolService.Delete(schoolID);
        }

        [HttpGet("excel")]
        public async Task<IActionResult> ExportSchools()
        {
            var schools = await _schoolService.GetSchools(null);
            using var stream = new MemoryStream();
            using var excelFile = new ExcelPackage(stream);
            var worksheet = excelFile.Workbook.Worksheets.Add("Schools");
            worksheet.Cells[1, 1].LoadFromCollection(schools, true, TableStyles.Light1);

            await excelFile.SaveAsAsync(stream);
            return File(stream.ToArray(), "application/octet-stream", "Schools.xlsx");


        }


        [HttpPost("excel")]
        public async Task<IActionResult> ImportSchools(IFormFile file)
        {
            using var excelFile = new ExcelPackage(file.OpenReadStream());
            var worksheet = excelFile.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                return BadRequest("No worksheet found in the uploaded file.");
            }

            var schools = new List<SchoolViewModel>();
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                int result;
                if (worksheet.Cells[row, 1].Text == "")
                {
                    result = _schoolService.Post(new SchoolViewModel
                    {
                        Name = worksheet.Cells[row, 2].Text,
                        Address = worksheet.Cells[row, 3].Text
                    });
                }
                else
                {
                    result = _schoolService.Put(new SchoolViewModel
                    {
                        Id = int.Parse(worksheet.Cells[row, 1].Text),
                        Name = worksheet.Cells[row, 2].Text,
                        Address = worksheet.Cells[row, 3].Text
                    });
                }
                if (result == -1)
                {
                    return BadRequest($"Error processing row {row}: School could not be saved.");
                }
            }
            //nếu thích thì chạy for để xóa =)))
            return Ok("Schools imported successfully.");
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> ExportSchoolPdf()
        {

            var htmlText = System.IO.File.ReadAllText("Template/SchoolReport.html");
            htmlText = htmlText.Replace("{{SchoolName}}", "Thuan Dep Trai");

            var rederEngine = new ChromePdfRenderer();

            var pdf = await rederEngine.RenderHtmlAsPdfAsync(htmlText);

            var path = Path.Combine(_fileInformation.RootDirectory, "SchoolReport.pdf");

            pdf.SaveAs(path);
            return Ok();
        }

        [HttpGet("pdf-dynamic")]
        public async Task<IActionResult> ExportSchoolPdfDynamic()
        {
            var schools = await _schoolService.GetSchools(null);

            var model = new SchoolReportModel
            {
                Author = "Thuan Dep Trai",
                DateCreated = DateTime.Now.ToString("dd/MM/yyyy"),
                Schools = schools.ToList()
            };
            // sử dụng RazorLight để build file cshtml từ model và trả về htmlText để ironpdf chuyển sang pdf
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Template"))//GetCurrentDirectory() trả về đường dẫn của project hiện tại
                .UseMemoryCachingProvider() // Cache file static để lần sau khỏi phải đọc một lần nữa
                .Build();

            string htmlText = await engine.CompileRenderAsync("SchoolReportDynamic.cshtml", model);

            var rederEngine = new ChromePdfRenderer();

            var pdf = await rederEngine.RenderHtmlAsPdfAsync(htmlText);

            //var path = Path.Combine(_fileInformation.RootDirectory, "SchoolReport.pdf");

            //pdf.SaveAs(path);

            //using var memoryStream = new MemoryStream();
            //await pdf.Stream.CopyToAsync(memoryStream);

            return File(pdf.BinaryData, "application/pdf", "SchoolReport.pdf");
        }

        [HttpGet("test-hangfire")]
        public IActionResult TestHangFire()
        {
            
            string jobId =  BackgroundJob.Enqueue<GenerateSchoolReportJob>(instanceJob => instanceJob.ExecuteAsync());

            string jobId2 = BackgroundJob.Schedule(() => Console.WriteLine("This is a delayed job!"), TimeSpan.FromSeconds(10));

            string jobId3 = BackgroundJob.ContinueJobWith(jobId2,
                () => Console.WriteLine("This job runs after the job two!"));

            BackgroundJob.ContinueJobWith(jobId3,
                () => Console.WriteLine("This job runs after the job three!"));
            return Ok();

        }
    }
}
