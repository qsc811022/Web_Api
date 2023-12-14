using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private readonly IWeatherService _weatherService;

		public ValuesController(IWeatherService weatherService)
		{
			_weatherService = weatherService;
		}

		[HttpGet("weather")]
		public async Task<IActionResult> GetWeatherForCity(string city)
		{
			try
			{
				var weatherData = await _weatherService.GetWeatherForCityAsync(city);
				return Ok(weatherData);
			}
			catch (Exception ex)
			{
				// 可以记录异常详情
				return StatusCode(500, "An error occurred while processing your request.");
			}
		}
	}
}
