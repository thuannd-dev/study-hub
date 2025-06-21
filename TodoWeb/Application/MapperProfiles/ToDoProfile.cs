using AutoMapper;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.CourseStudentModel;
using TodoWeb.Application.Dtos.GradeStudentModel;
using TodoWeb.Application.Dtos.QuestionModel;
using TodoWeb.Application.Dtos.SchoolModel;
using TodoWeb.Application.Dtos.StudentModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.MapperProfiles
{
    public class ToDoProfile : Profile
    {
        public ToDoProfile() {

            //tất cả mapping thì sẽ để trong constructor này
            //map course -> courseviewmodel
            CreateMap<Course, CourseViewModel>()
                //CourseViewModel.CourseID = Course.ID
                .ForMember(dest => dest.CourseId, config => config.MapFrom(src => src.Id))
                .ForMember(dest => dest.CourseName, config => config.MapFrom(src => src.Name));
                //.ReverseMap();

            //map từ PostCourseViewModel -> Course
            CreateMap<PostCourseViewModel, Course>()
                .ForMember(dest => dest.Name, config => config.MapFrom(src => src.CourseName));

            //map từ CourseViewModel -> Course
            CreateMap<CourseViewModel, Course>()
                .ForMember(dest => dest.Id, config => config.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.Name, config =>
                {
                    config.PreCondition(src => !string.IsNullOrWhiteSpace(src.CourseName));
                    config.MapFrom(src => src.CourseName);
                });

            //map từ Student -> StudentViewModel
            CreateMap<Student, StudentViewModel>()
                .ForMember(dest => dest.FullName, config => config.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.SchoolName, config => config.MapFrom(src => src.School.Name))
                .ForMember(dest => dest.Address, static config => config.MapFrom(static src => new Address
                {
                    Street = src.Address1 ?? String.Empty,
                    ZipCode = src.Address2 ?? String.Empty
                }));

            //map từ CourseStudent -> CourseViewModel
            CreateMap<CourseStudent, CourseViewModel>()
                .ForMember(dest => dest.CourseName, config => config.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.StartDate, config => config.MapFrom(src => src.Course.StartDate));

            //map từ Student -> StudentCourseDetailViewModel
            CreateMap<Student, StudentCourseDetailViewModel>()
                .ForMember(dest => dest.StudentId, config => config.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentName,
                    config => config.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Courses,
                    config => config.MapFrom(src => src.CourseStudent));//nó sẽ tự map từ CourseStudent sang CourseViewModel
                                                                        //.ForMember(dest => dest.Courses, config => config.MapFrom(src => src.CourseStudent.Select(cs => new CourseViewModel
                                                                        // {
                                                                        //     CourseId = cs.CourseId,
                                                                        //     CourseName = cs.Course.Name
                                                                        // })));


            //map từ CourseStudent -> StudentViewModel
            //CreateMap<CourseStudent, StudentViewModel>()
            //    .ForMember(dest => dest.Id, config => config.MapFrom(src => src.StudentId))
            //    .ForMember(dest => dest.FullName,
            //        config => config.MapFrom(src => src.Student.FirstName + " " + src.Student.LastName))
            //    .ForMember(dest => dest.Age, config => config.MapFrom(src => src.Student.Age))
            //    .ForMember(dest => dest.Balance, config => config.MapFrom(src => src.Student.Balance))
            //    .ForMember(dest => dest.SchoolName, config => config.MapFrom(src => src.Student.School.Name));//1 student sẽ có 1 school nhưng 1 school có nhiều student
            //Chấm trực tiếp được là bởi vì trong Strudent có cột SId, nó cho phép mỗi Student sẽ tham chiếu trực tiếp đến School thông qua SId

            //map từ Course -> CourseStudentDetailViewModel
            //CreateMap<Course, CourseStudentDetailViewModel>()
            //    .ForMember(dest => dest.CourseId, config => config.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.CourseName, config => config.MapFrom(src => src.Name))
            //    .ForMember(dest => dest.Students, config => config.MapFrom(src => src.CourseStudent));
            CreateMap<Course, CourseStudentDetailViewModel>()
                .ForMember(dest => dest.CourseId, config => config.MapFrom(src => src.Id))
                .ForMember(dest => dest.CourseName, config => config.MapFrom(src => src.Name))
                .ForMember(dest => dest.StartDate, config => config.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.Students, config => config.MapFrom(src => src.CourseStudent.Select(cs => cs.Student)));
            //1 courseStudent sẽ có 1 student nhưng 1 student có nhiều courseStudent
            //Không chấm trực tiếp được bởi vì trong courseStudent vừa có cột StudentId vừa có cột CourseId => ambiguous, nó không cho phép trực tiếp tham chiếu đến Student hoặc Course 




            //map từ PostCourseStudentViewModel -> CourseStudent
            CreateMap<PostCourseStudentViewModel, CourseStudent>();

            //map từ Students -> StudentCourseGradeViewModel
            CreateMap<Student, StudentCourseGradeViewModel>()
                .ForMember(dest => dest.StudentId, config => config.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentName, config => config.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.CourseScore, config => config.MapFrom(src => src.CourseStudent));

            //map từ CourseStudent -> CourseGradeViewModel
            CreateMap<CourseStudent, CourseGradeViewModel>()
                .ForMember(dest => dest.CourseName, config => config.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.AssignmentScore, config => config.MapFrom(src => src.Grade.AssignmentScore))
                .ForMember(dest => dest.FinalScore, config => config.MapFrom(src => src.Grade.FinalScore))
                .ForMember(dest => dest.PracticalScore, config => config.MapFrom(src => src.Grade.PracticalScore));



            //map từ StudentCourseGradeViewModel -> StudentCourseGradeWithAverageCourseScoreViewModel
            CreateMap<StudentCourseGradeViewModel, StudentCourseGradeWithAverageCourseScoreViewModel>()
                .ForMember(dest => dest.StudentCourseGradeViewModel, config => config.MapFrom(src => src));
  

            //CreateMap<StudentCourseGradeViewModel, StudentCourseGradeWithAverageCourseScoreViewModel>()
            //    .ForMember(dest => dest.StudentCourseGradeViewModel, opt => opt.MapFrom(src => src))
            //    .ForMember(dest => dest.AverageCoursesScore, opt => opt.MapFrom(src => src.CourseScore.Any()
            //        ? src.CourseScore.Average(cs => (cs.AssignmentScore + cs.PracticalScore + cs.FinalScore) / 3)
            //        : (decimal?)null));

            //map từ School -> SchoolViewModel
            CreateMap<School, SchoolViewModel>();

            //map từ SchoolViewModel -> School
            CreateMap<SchoolViewModel, School>()
                .ForMember(dest => dest.Id, config => config.Ignore());

            //map từ School -> SchoolStudentViewModel
            CreateMap<School, SchoolStudentViewModel>();

            //map từ StudentViewModel -> Student
            CreateMap<StudentViewModel, Student>();

            //map từ Question -> QuestionViewModel
            CreateMap<Question, QuestionViewModel>()
                .ForMember(dest => dest.QuestionId, config => config.MapFrom(src => src.Id));

            //map từ QuestionPostModel -> Question
            CreateMap<QuestionPostModel, Question>();

            //map từ QuestionPutModel -> Question
            CreateMap<QuestionPutModel, Question>()
                .ForMember(dest => dest.Id, config => config.MapFrom(src => src.QuestionId));






        }
    }
}
