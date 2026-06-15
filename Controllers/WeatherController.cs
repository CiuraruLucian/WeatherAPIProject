using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace WeatherAPI.Controllers
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {

        // Insert here the dependency injection after you get the Api Key
        private readonly IConfiguration _configuration;

        private readonly IConnectionMultiplexer _connection;

        private readonly IDatabase _redisdb;
        public WeatherController(IConfiguration configuration, IConnectionMultiplexer connection)
        {
            _configuration = configuration;
            _connection = connection;
            _redisdb = connection.GetDatabase();
        }
        [HttpGet("{city}")]
        public IActionResult Get(string city)
        {
            try
            {
                var cachedValue = _redisdb.StringGet(city);
                if(cachedValue.HasValue)
                {
                    return Ok(cachedValue.ToString());
                }
                else
                {
                    _redisdb.StringSet(city, "Cached value for city", TimeSpan.FromHours(12));
                    var weather = new { City = city, Temperature = 22, Humidity = 60, Condition = "Sunny" };
                    return Ok(weather);

                }
            }
            catch (Exception ex)
            {
                return StatusCode (500, new {error = ex.Message});
            }
        }
    }
}