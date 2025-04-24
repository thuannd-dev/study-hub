using Serilog;
using TodoWeb.Application.Dtos.GuidModel;
using TodoWeb.Application.MapperProfiles;
using TodoWeb.Application.Services;
using TodoWeb.Application.Services.Courses;
using TodoWeb.Application.Services.CourseStudents;
using TodoWeb.Application.Services.CourseStudents;
using TodoWeb.Application.Services.ExamQuestions;
using TodoWeb.Application.Services.Exams;
using TodoWeb.Application.Services.ExamSubmissionDetails;
using TodoWeb.Application.Services.ExamSubmissions;
using TodoWeb.Application.Services.Grade;
using TodoWeb.Application.Services.Questions;
using TodoWeb.Application.Services.School;
using TodoWeb.Application.Services.Students;
using TodoWeb.Application.Services.Students;
using TodoWeb.Infrastructures;
//file program la file khi project build ra chay dau tien
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();//register dependencies injection, nó gồm có một interface hoặc abstract class, một class implement, 
//và chúng ta register nó trong một DI collection
//Khi app được start lên thì cái app sẽ đi collect-gom tất cả các dependencies injection đã được register vào trong cái DI Collection 
//Khi cần dùng thì nó sẽ tự NEW cho mình
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddTransient<IGuidGenerator, GuidGenerator>();
builder.Services.AddSingleton<ISingletonGenerator, SingltonGenerator>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddSingleton<GuidData>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<ICourseStudentService, CourseStudentService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IExamQuestionService, ExamQuestionService>();
builder.Services.AddScoped<IExamSubmissionDetailsService, ExamSubmissionDetailsService>();
builder.Services.AddScoped<IExamSubbmissionService, ExamSubbmissionService>();
builder.Services.AddAutoMapper(typeof(ToDoProfile));
builder.Services.AddAutoMapper(typeof(ExamProfile));
builder.Services.AddAutoMapper(typeof(ExamQuestionProfile));
builder.Services.AddAutoMapper(typeof(ExamSubmissionProfile));
builder.Services.AddAutoMapper(typeof(ExamSubmissionDetailsProfile));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.File("C:\\Users\\DELL\\OneDrive\\Desktop\\Logs\\log.txt",
        rollingInterval: RollingInterval.Minute)
    .CreateLogger();
builder.Host.UseSerilog();

//DI Containers, IServiceProvider
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
