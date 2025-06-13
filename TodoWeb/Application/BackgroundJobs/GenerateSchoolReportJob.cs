using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using RazorLight;
using TodoWeb.Application.Dtos.SchoolModel;
using TodoWeb.Application.Services.School;
using TodoWeb.Domains.AppsettingsConfigurations;

namespace TodoWeb.Application.BackgroundJobs
{
    public class GenerateSchoolReportJob
    {
        private readonly ISchoolService _schoolService;
        private readonly FileInformation _fileInformation;
        public GenerateSchoolReportJob(ISchoolService schoolService, IOptions<FileInformation> fileInformation)
        {
            _schoolService = schoolService;
            _fileInformation = fileInformation.Value;
        }
        public async Task ExecuteAsync()
        {
            var schools = await _schoolService.GetSchools(null);

            var model = new SchoolReportModel
            {
                Author = "Thuan Dep Trai",
                DateCreated = DateTime.Now.ToString("dd/MM/yyyy"),
                Schools = schools.ToList()
            };

            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Template"))
                .UseMemoryCachingProvider() // Cache file static để lần sau khỏi phải đọc - tạo
                .Build();

            string htmlText = await engine.CompileRenderAsync("SchoolReportDynamic.cshtml", model);

            var rederEngine = new ChromePdfRenderer();

            var pdf = await rederEngine.RenderHtmlAsPdfAsync(htmlText);

            var path = Path.Combine(_fileInformation.RootDirectory,
                $"SchoolReport-{DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss")}.pdf");

            pdf.SaveAs(path);
        }
    }
}
