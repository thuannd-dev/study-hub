using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using TodoWeb.Application.ActionFilters;
using TodoWeb.Application.Dtos.SchoolModel;
using TodoWeb.Application.Services.School;
using TodoWeb.Constants.Enums;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[TypeFilter(typeof(AuthorizationFilter), Arguments = [new Role[] { Role.Admin, Role.User }])]
    [Authorize(Roles = "User, Admin")]//atribute này ko phục vụ cho session
    public class SchoolController : Controller
    {
        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        public IEnumerable<SchoolViewModel> GetSchools(int? schoolId)
        {
            
            return _schoolService.GetSchools(schoolId);
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
            var schools = _schoolService.GetSchools(null);
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
                }else
                {
                    result = _schoolService.Put(new SchoolViewModel
                    {
                        Id = int.Parse(worksheet.Cells[row, 1].Text),
                        Name = worksheet.Cells[row, 2].Text,
                        Address = worksheet.Cells[row, 3].Text
                    });
                }
                if(result == -1)
                {
                    return BadRequest($"Error processing row {row}: School could not be saved.");
                }
            }
            //nếu thích thì chạy for để xóa =)))
            return Ok("Schools imported successfully.");
        }

    }
}
