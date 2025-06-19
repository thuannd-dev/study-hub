using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using Serilog;
using TodoWeb.Application.ActionFilters;
using TodoWeb.Application.BackgroundJobs;
using TodoWeb.Application.Dtos.GuidModel;
using TodoWeb.Application.MapperProfiles;
using TodoWeb.Application.Middleware;
using TodoWeb.Application.Services;
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
using TodoWeb.Domains.AppsettingsConfigurations;
using TodoWeb.Infrastructures;
using TodoWeb.Infrastructures.Extensions;
//file program la file khi project build ra chay dau tien
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(option =>
{
    option.Filters.Add<TestFilter>();
});

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddHttpClient();//để sử dụng IHttpClientFactory của FacebookCredentialService,
                                 //dùng để tạo ra các HttpClient để gọi các API bên ngoài, không nên dùng HttpClient trực tiếp vì nó sẽ giữ lại kết nối,
                                 //tạo nhiều instance dư thừa => không tối ưu

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ToDoApp",
        Version = "v1",
        Description = "ToDoApp API",
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a token",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };
    option.AddSecurityDefinition("Bearer", securityScheme);
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});
builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();//register dependencies injection, nó gồm có một interface hoặc abstract class, một class implement, 
//và chúng ta register nó trong một DI collection
//Khi app được start lên thì cái app sẽ đi collect-gom tất cả các dependencies injection đã được register vào trong cái DI Collection 
//Khi cần dùng thì nó sẽ tự NEW cho mình
//

//dbcontext có thể là transient nhưng không tối uuw 
//dbcontext không thể là singleton vì nó sẽ giữ lại trạng thái của các entity, nếu là singleton thì nó sẽ dùng xuyên suốt cả app, 
//=> lưu rất nhiều state => rất nặng => không tối ưu
//thread safe là tức là nó có thể chạy đồng thời nhiều request mà không bị ảnh hưởng đến nhau,
//2. dbcontext không là thread safe (không thể có 2 service truy cập cùng lúc) => không thể là singleton


//dbcontext => dùng scoped, scoped là tối ưu nhất vì nó sẽ tạo ra một instance mới cho mỗi request, mỗi request sẽ có một dbcontext khác nhau => không bị ảnh hưởng đến nhau
//builder.Services.AddScoped<IToDoService, ToDoService>();
//builder.Services.AddTransient<IGuidGenerator, GuidGenerator>();
//builder.Services.AddSingleton<ISingletonGenerator, SingltonGenerator>();
//builder.Services.AddScoped<IStudentService, StudentService>();
//builder.Services.AddScoped<ISchoolService, SchoolService>();
//builder.Services.AddSingleton<GuidData>();
//builder.Services.AddScoped<ICourseService, CourseService>();
//builder.Services.AddScoped<IGradeService, GradeService>();
//builder.Services.AddScoped<ICourseStudentService, CourseStudentService>();
//builder.Services.AddScoped<IQuestionService, QuestionService>();
//builder.Services.AddScoped<IExamService, ExamService>();
//builder.Services.AddScoped<IExamQuestionService, ExamQuestionService>();
//builder.Services.AddScoped<IExamSubmissionDetailsService, ExamSubmissionDetailsService>();
//builder.Services.AddScoped<IExamSubbmissionService, ExamSubbmissionService>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddAutoMapper(typeof(ToDoProfile));
//builder.Services.AddAutoMapper(typeof(ExamProfile));
//builder.Services.AddAutoMapper(typeof(ExamQuestionProfile));
//builder.Services.AddAutoMapper(typeof(ExamSubmissionProfile));
//builder.Services.AddAutoMapper(typeof(ExamSubmissionDetailsProfile));
//builder.Services.AddAutoMapper(typeof(UserProfile));
//builder.Services.AddSingleton<LogMiddleware>();
//builder.Services.AddSingleton<RateLimitMiddleware>();
////builder.Services.AddSingleton<LogFilter>();
//builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddService();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);//sau 30s không có request nào thì session sẽ bị xóa
    options.Cookie.HttpOnly = true;//không cho client truy cập vào cookie này
    options.Cookie.IsEssential = true;
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

var googleSettings = builder.Configuration.GetSection("GooggleAuthentication");    
builder.Services.Configure<GoogleSettings>(googleSettings);

var facebookSettings = builder.Configuration.GetSection("FacebookAuthentication");
builder.Services.Configure<FacebookSettings>(facebookSettings);

var fileInformation = builder.Configuration.GetSection("FileInformation");
builder.Services.Configure<FileInformation>(fileInformation);

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
//    {
//        option.Cookie.HttpOnly = true;//không cho js truy cập vào cookie này để tránh bị tấn công XSS
//        option.ExpireTimeSpan = TimeSpan.FromSeconds(30);
//        option.SlidingExpiration = true;//làm mới thời gian hết hạn của cookie khi request lại
//        option.Cookie.SecurePolicy = CookieSecurePolicy.Always;

//        option.Events = new CookieAuthenticationEvents
//        {
//            //nếu ko authen đc
//            OnRedirectToLogin = context =>
//            {
//                context.Response.StatusCode = 401;
//                return Task.CompletedTask;
//            },
//            //nếu author ko đc
//            OnRedirectToAccessDenied = context =>
//            {
//                context.Response.StatusCode = 403;
//                return Task.CompletedTask;
//            }
//        };
//    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,//Validate the server 
        ValidateAudience = true,//Validate the recipient of the token is authorized to receive
        ValidateLifetime = true,//Check if the token is not expired and the signing key of the issuer is valid
        ValidateIssuerSigningKey = true,//Validate signature of the token 
        ClockSkew = TimeSpan.Zero,//độ trễ cho phép giữa server và client là 0
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"]
    };
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.File("C:\\Users\\DELL\\OneDrive\\Desktop\\Logs\\log.txt",
        rollingInterval: RollingInterval.Minute)
    .CreateLogger();
builder.Host.UseSerilog();

ExcelPackage.License.SetNonCommercialPersonal("Thuan");

builder.Services.AddHangfire(x => x.UseSqlServerStorage("Server=DESKTOP-TUDP88B\\SQLEXPRESS;Database=ToDoApp;Trusted_Connection=True;TrustServerCertificate=True"));
builder.Services.AddHangfireServer();

//DI Containers, IServiceProvider
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseExceptionHandler("/Error");


app.UseHttpsRedirection();
//authen phải trước authorization vì đây là middleware xác thực người dùng
app.UseAuthentication();
app.UseMiddleware<RevokeCheckMiddleware>();
app.UseAuthorization();
app.UseSession();
app.MapControllers();


app.UseCors(option =>
    option.WithOrigins("http://localhost:3000", "https://localhost:7016")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    //Credentials include cookies and HTTP authentication schemes
    //Mặc định, các truy vấn CORS không gửi hoặc thiết lập bất cứ cookie nào trên trình duyệt. Nếu muốn sử dụng cookie trong truy vấn đó,
    //chúng ta phải đặt thuộc tính withCredentials của truy vấn bằng true
    //AllowCredentials() sẽ cho phép cookie được gửi trong truy vấn CORS, nếu không có thì sẽ không gửi cookie trong truy vấn CORS
    .SetPreflightMaxAge(TimeSpan.FromSeconds(10))
    //đặt thời gian tối đa cho preflight request, nếu không thì sẽ gửi preflight request mỗi lần truy vấn CORS
    //mặc định là 5 giây nhưng nhiều browser set cao hơn(chrome là 2 tiếng), nhưng có thể đặt cao hơn nếu cần thiết
    //.WithExposedHeaders("X-Total-Count", "X-Page-Size") //expose headers cho client, nếu không thì client sẽ không thể truy cập được các header này


);

//10 request trong 30s
app.UseMiddleware<RateLimitMiddleware>();

app.Use(async (context, next) =>
{
    Console.WriteLine("Request to middleware 1");
    await next(context);
    Console.WriteLine("Response to middleware 1");
});


app.Use(async (context, next) =>
{
    Console.WriteLine("Request to middleware 2");
    await next(context);
    Console.WriteLine("Response to middleware 2");
});

app.UseMiddleware<LogMiddleware>();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    IgnoreAntiforgeryToken = true                                 // <--This
});

//thường cần được gọi một lần lúc ứng dụng khởi động, nên thường vẫn sẽ nằm ở Program hoặc Startup.
RecurringJob.AddOrUpdate<GenerateSchoolReportJob>("GenerateSchoolReport",
    instanceJob => instanceJob.ExecuteAsync(), Cron.Minutely);


app.Run();
