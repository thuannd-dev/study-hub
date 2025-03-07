using Microsoft.AspNetCore.Mvc;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        //  
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //POST:
        //PUT: Idempotency
        //Idempotency goi post 10 lan khac goi put 10 lan, put 10 lần như 1, còn post 10 lần tạo mới 10
        //Front end -> Backend -> Back create data
        //Front < kq Back -> nếu dùng post sẽ bị duplicate 
        //đối với put thì ko 

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return new List<WeatherForecast> 
            {
                new WeatherForecast
                {
                    Date = DateTime.Now.Date,
                    TemperatureC = 37,
                    Summary = "Hot"
                }
                
            };
        }
    }
}
