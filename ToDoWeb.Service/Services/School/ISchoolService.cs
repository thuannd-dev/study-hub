using TodoWeb.Application.Dtos.SchoolModel;

namespace TodoWeb.Application.Services.School
{
    public interface ISchoolService
    {
        public Task<IEnumerable<SchoolViewModel>> GetSchools(int? schoolId);
        public SchoolStudentViewModel GetSchoolDetail(int schoolId);
        public int Post(SchoolViewModel school);
        public int Put(SchoolViewModel school);
        public int Delete(int schoolId);
    }
}
