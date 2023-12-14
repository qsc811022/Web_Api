using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WeatherBureauController : ControllerBase
	{
		private readonly ILogger<WeatherBureauController> _logger;
		private readonly IWeatherService _weatherService;

		public WeatherBureauController(ILogger<WeatherBureauController> logger, IWeatherService weatherService)
		{
			_logger = logger;
			_weatherService = weatherService;
		}

		[HttpGet("{city}")]
		public async Task<IActionResult> GetWeatherForCity(string city)
		{
			try
			{
				var weatherData = await _weatherService.GetWeatherForCityAsync(city);
				return Ok(weatherData);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting weather data for {City}", city);
				return StatusCode(500, "An error occurred while processing your request.");
			}
		}
	}
}
