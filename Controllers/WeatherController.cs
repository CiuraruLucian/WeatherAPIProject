using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {

        // Insert here the dependency injection after you get the Api Key
        [HttpGet("{city}")]
        public IActionResult Get(string city)
        {
            try
            {
                var weather = new { City = city, Temperature = 22, Condition = "Sunny", Humidity = 60 };

                return Ok(weather);
            }
            catch (Exception ex)
            {
                return StatusCode (500, new {error = ex.Message});
            }
        }
    }
}