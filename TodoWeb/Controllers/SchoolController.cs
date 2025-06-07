using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}
