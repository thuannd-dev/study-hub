using TodoWeb.Application.Dtos.GuidModel;
using TodoWeb.Application.Services;
using TodoWeb.Application.Services.Course;
using TodoWeb.Application.Services.CourseStudent;
using TodoWeb.Application.Services.Grade;
using TodoWeb.Application.Services.School;
using TodoWeb.Application.Services.Student;
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
