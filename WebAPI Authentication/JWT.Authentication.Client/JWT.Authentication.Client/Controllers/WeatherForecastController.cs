using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Authentication.Client.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            // to get claims
            var userId = User.Claims.FirstOrDefault(s => s.Type == "UserId").Value;
            // to get token
            var token = await HttpContext.GetTokenAsync("access_token");

            Console.WriteLine("UserId :: " + userId);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public IActionResult OnlyForAdmin() 
        {
            return Ok("This endpoint is accessible only for admin");
        }

        [HttpGet]
        [Route("openapi"), AllowAnonymous]
        public IActionResult OpenAPiEndpoint()
        {
            return Ok("This endpoint is open any one can access it.");
        }
    }
}