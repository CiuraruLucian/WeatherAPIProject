using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StackExchange.Redis;
using System.Net.Http;

namespace WeatherAPI.Controllers
{
    [Route("api/weather")]
    
    [ApiController]
    
    [EnableRateLimiting("fixed")]
    public class WeatherController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        private readonly IConnectionMultiplexer _connection;

        private readonly IDatabase _redisdb;

        private readonly HttpClient _httpClient;

        
        public WeatherController(IConfiguration configuration, IConnectionMultiplexer connection, HttpClient httpClient)
        {
            _configuration = configuration;
            _connection = connection;
            _redisdb = connection.GetDatabase();
            _httpClient = httpClient;
        }
        [HttpGet("{city}")]
        public async Task<IActionResult> Get(string city)
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
                    var apiKey = _configuration["VisualCrossing:ApiKey"];
                    var url = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{city}?key={apiKey}";
                    var response = await _httpClient.GetStringAsync(url);
                    _redisdb.StringSet(city, response, TimeSpan.FromHours(12));
                    return Ok(response);

                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode (500, new {error = "External API unavailable"});
            }
        }
    }
}