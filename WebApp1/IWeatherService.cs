namespace WebApp1
{
	public interface IWeatherService
	{
		Task<WeatherData> GetWeatherForCityAsync(string city);
	}
}
