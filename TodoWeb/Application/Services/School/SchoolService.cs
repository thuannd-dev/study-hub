using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.Dtos.SchoolModel;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.School
{
    public class SchoolService : ISchoolService
    {
        private readonly IApplicationDbContext _context;

        public SchoolService(IApplicationDbContext context)
        {
            _context = context;
        }


        public IEnumerable<SchoolViewModel> GetSchools(int? schoolId)
        {
            var query = _context.School
                .AsQueryable();//build leen 1 cau query

            if (schoolId.HasValue)
            {

                query = query.Where(x => x.Id == schoolId);//add theem ddk 
            }

            return query.Select(x => new SchoolViewModel
            {
                Id = x.Id,
                Address = x.Address,
                Name = x.Name,

            }).ToList();//khi minhf chaams to list thi entity framework moi excute cau query 
            //chua to list thif se build tren memory
        }

        public int Post(SchoolViewModel school)
        {
            // kiểm tra xem school id có bị trùng hay không
            var dupID = _context.School.Find(school.Id);
            if (dupID != null || school.Id < 1)
            {
                return -1;
            }
            // lấy school nhờ vào school name
            var existingSchool = _context.School.FirstOrDefault(s => s.Name.Equals(school.Name)); // không dùng where bởi vì tìm ra một list

            if (existingSchool != null)
            {
                return -1;
            }

            var data = new Domains.Entities.School
            {
                Name = school.Name,
                Address = school.Address,
            };
            _context.School.Add(data);
            _context.SaveChanges();
            return data.Id;
        }

        public int Put(SchoolViewModel school)
        {
            var data = _context.School.Find(school.Id);
            //tìm student
            if (data == null)
            {
                return -1;
            }
            var name = school.Name.Split(' ');
            //kiểm tra xem người dùng có đưa đúng tên school
            var existingSchool = _context.School.FirstOrDefault(s => s.Name.Equals(school.Name));//không dùng where bởi vì tìm ra một list
            if (existingSchool != null)
            {
                return -1;
            }
            data.Name = school.Name;
            data.Address = school.Address;
            _context.SaveChanges();
            return data.Id;
        }

        public int Delete(int schoolId)
        {
            var data = _context.School.Find(schoolId);
            if (data == null)
            {
                return -1;
            }
            _context.School.Remove(data);
            _context.SaveChanges();
            return 0;
        }
    }
}
