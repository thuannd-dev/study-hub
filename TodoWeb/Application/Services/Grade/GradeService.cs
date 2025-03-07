using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.Dtos.GradeStudentModel;
using TodoWeb.Application.Services.CourseStudent;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.Grade
{
    public class GradeService : IGradeService
    {
        private readonly IApplicationDbContext _context;
        private readonly ICourseStudentService _courseStudentService;
        public GradeService(IApplicationDbContext context, ICourseStudentService courseStudentService)
        {
            _context = context;
            _courseStudentService = courseStudentService;
        }

        public IEnumerable<StudentCourseGradeWithAverageCourseScoreViewModel> GetAverageGradeOfStudents(int? studentId)
        {
            var data = GetGradeOfStudents(studentId, null);
            return data.Select(studentGrade => new StudentCourseGradeWithAverageCourseScoreViewModel
            {
                StudentCourseGradeViewModel = studentGrade
            });
        }

        public IEnumerable<StudentCourseGradeViewModel> GetGradeOfStudents(int? studentId, int? courseId)
        {

            //var query = _context.CourseStudent
            //    .Include(x => x.Student)
            //    .Include(x => x.Course)
            //    .Include(x => x.Grade)
            //    .AsQueryable();

            //if (studentId.HasValue)
            //{
            //    query = query.Where(x => x.StudentId == studentId);
            //}

            //if (courseId.HasValue)
            //{
            //    query = query.Where(x => x.CourseId == courseId);
            //}

            //return 


            var query = _context.Students.AsQueryable();
            if (studentId.HasValue)
            {
                query = query.Where(student => student.Id == studentId);
            }

            query = query.Include(student => student.CourseStudent)
                         .ThenInclude(cs => cs.Course);

            if (courseId.HasValue)
            {
                return query.Select(student => new StudentCourseGradeViewModel
                {
                    StudentId = student.Id,
                    StudentName = student.FirstName + " " + student.LastName,

                    CourseScore = student.CourseStudent.Where(course => course.CourseId == courseId)
                .Select(cs => new CourseGradeViewModel
                {
                    CourseId = cs.CourseId,
                    CourseName = cs.Course.Name,
                    AssignmentScore = cs.Grade.AssignmentScore,
                    PracticalScore = cs.Grade.PracticalScore,
                    FinalScore = cs.Grade.FinalScore,

                }).ToList()
                }).ToList();
            }

            return query.Select(student => new StudentCourseGradeViewModel
            {
                StudentId = student.Id,
                StudentName = student.FirstName + " " + student.LastName,

                CourseScore = student.CourseStudent
                .Select(cs => new CourseGradeViewModel
                {
                    CourseId = cs.CourseId,
                    CourseName = cs.Course.Name,
                    AssignmentScore = cs.Grade.AssignmentScore,
                    PracticalScore = cs.Grade.PracticalScore,
                    FinalScore = cs.Grade.FinalScore,

                }).ToList()
            }).ToList();
            //return query.Select(student => new StudentCourseGradeViewModel
            //{
            //    StudentId = student.Id,
            //    StudentName = student.FirstName + " " + student.LastName,

            //    courseScore = (courseId.HasValue ? student.CourseStudent.Where(course => course.CourseId == courseId) : student.CourseStudent)
            //    .Select(cs => new CourseGradeViewModel
            //    {
            //        CourseId = cs.CourseId,
            //        CourseName = cs.Course.Name,
            //        AssignmentScore = cs.Grade.AssignmentScore,
            //        PracticalScore = cs.Grade.PracticalScore,
            //        FinalScore = cs.Grade.FinalScore,

            //    }).ToList()
            //}).ToList();




        }

        public int PostGradeStudent(PostGradeViewModel gradeViewModel)
        {
            var validAssignmentScore = gradeViewModel.AssignmentScore >= 0 && gradeViewModel.AssignmentScore <= 100 ? true : false;
            var validPracticalScore = gradeViewModel.PracticalScore >= 0 && gradeViewModel.PracticalScore <= 100 ? true : false;
            var validFinalScore = gradeViewModel.FinalScore >= 0 && gradeViewModel.FinalScore <= 100 ? true : false;
            if(!validAssignmentScore || !validPracticalScore || !validFinalScore)
            {
                return -1;
            }

            var courseStudentId = _courseStudentService.PostCourseStudent(new Dtos.CourseStudentModel.PostCourseStudentViewModel
            {
                StudentId = gradeViewModel.StudentId,
                CourseId = gradeViewModel.CourseId
            });
            if(courseStudentId == -1)
            {
                return -1;
            }

            var data = new Domains.Entities.Grade
            {
                CourseStudentId = courseStudentId,
                AssignmentScore = gradeViewModel.AssignmentScore,
                PracticalScore = gradeViewModel.PracticalScore,
                FinalScore = gradeViewModel.FinalScore
            };
            _context.Grades.Add(data);
            _context.SaveChanges();
            return data.Id;
        }

        public int PutGradeStudent(PostGradeViewModel gradeViewModel)
        {
            var hasCourseStudent = _context.CourseStudent
                .FirstOrDefault(cs => cs.StudentId == gradeViewModel.StudentId && cs.CourseId == gradeViewModel.CourseId);
            var validAssignmentScore = gradeViewModel.AssignmentScore >= 0 && gradeViewModel.AssignmentScore <= 100 ? true : false;
            var validPracticalScore = gradeViewModel.PracticalScore >= 0 && gradeViewModel.PracticalScore <= 100 ? true : false;
            var validFinalScore = gradeViewModel.FinalScore >= 0 && gradeViewModel.FinalScore <= 100 ? true : false;
            if (hasCourseStudent == null || !validAssignmentScore || !validPracticalScore || !validFinalScore)
            {
                return -1;
            }

            var grade = _context.Grades.FirstOrDefault(grade => grade.CourseStudentId == hasCourseStudent.Id);
            if(grade == null)
            {
                grade = new Domains.Entities.Grade
                {
                    CourseStudentId = hasCourseStudent.Id,
                    AssignmentScore = gradeViewModel.AssignmentScore,
                    PracticalScore = gradeViewModel.PracticalScore,
                    FinalScore = gradeViewModel.FinalScore
                };
                _context.Grades.Add(grade);
                _context.SaveChanges();
                return grade.Id;
            }
            grade.AssignmentScore = gradeViewModel.AssignmentScore;
            grade.PracticalScore = gradeViewModel.PracticalScore;
            grade.FinalScore = gradeViewModel.FinalScore;
            _context.SaveChanges();
            return grade.Id;
        }
    }
}
