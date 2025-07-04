using System.Runtime.CompilerServices;
using TodoWeb.Application.Dtos.GuidModel;
using TodoWeb.Application.MapperProfiles;
using TodoWeb.Application.Middleware;
using TodoWeb.Application.Services.CacheService;
using TodoWeb.Application.Services.Courses;
using TodoWeb.Application.Services.CourseStudents;
using TodoWeb.Application.Services.ExamQuestions;
using TodoWeb.Application.Services.Exams;
using TodoWeb.Application.Services.ExamSubmissionDetails;
using TodoWeb.Application.Services.ExamSubmissions;
using TodoWeb.Application.Services.Grade;
using TodoWeb.Application.Services.Questions;
using TodoWeb.Application.Services.School;
using TodoWeb.Application.Services.Students;
using TodoWeb.Application.Services.Users;
using TodoWeb.Application.Services;
using TodoWeb.Application.Services.Users.GoogleService;
using TodoWeb.Application.Services.Users.FacebookService;
using ToDoWeb.DataAccess.Repositories.CourseAccess;
using ToDoWeb.DataAccess.Repositories.StudentAccess;

namespace TodoWeb.Infrastructures.Extensions
{
    public static class AddDependencyInjection
    {

        public static void AddService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IToDoService, ToDoService>();
            serviceCollection.AddTransient<IGuidGenerator, GuidGenerator>();
            serviceCollection.AddSingleton<ISingletonGenerator, SingltonGenerator>();
            serviceCollection.AddScoped<IStudentService, StudentService>();
            serviceCollection.AddScoped<ISchoolService, SchoolService>();
            serviceCollection.AddSingleton<GuidData>();
            serviceCollection.AddScoped<ICourseService, CourseService>();
            serviceCollection.AddScoped<IGradeService, GradeService>();
            serviceCollection.AddScoped<ICourseStudentService, CourseStudentService>();
            serviceCollection.AddScoped<IQuestionService, QuestionService>();
            serviceCollection.AddScoped<IExamService, ExamService>();
            serviceCollection.AddScoped<IExamQuestionService, ExamQuestionService>();
            serviceCollection.AddScoped<IExamSubmissionDetailsService, ExamSubmissionDetailsService>();
            serviceCollection.AddScoped<IExamSubbmissionService, ExamSubbmissionService>();
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddAutoMapper(typeof(ToDoProfile));
            serviceCollection.AddAutoMapper(typeof(ExamProfile));
            serviceCollection.AddAutoMapper(typeof(ExamQuestionProfile));
            serviceCollection.AddAutoMapper(typeof(ExamSubmissionProfile));
            serviceCollection.AddAutoMapper(typeof(ExamSubmissionDetailsProfile));
            serviceCollection.AddAutoMapper(typeof(UserProfile));
            serviceCollection.AddAutoMapper(typeof(CourseProfile));
            serviceCollection.AddSingleton<LogMiddleware>();
            serviceCollection.AddSingleton<RateLimitMiddleware>();
            serviceCollection.AddSingleton<RevokeCheckMiddleware>();
            //serviceCollection.AddSingleton<LogFilter>();
            serviceCollection.AddSingleton<ICacheService, CacheService>();
            serviceCollection.AddSingleton<IGoogleCredentialService, GoogleCredentialService>();
            serviceCollection.AddSingleton<IFacebookCredentialService, FacebookCredentialService>();
            serviceCollection.AddScoped<ICourseRepository, CourseRepository>();
            serviceCollection.AddScoped<IStudentRepository, StudentRepository>();

        }
    }
}
