using System.Text.Json;

namespace WebApp1
{
	public class WeatherService : IWeatherService
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiKey = "CWA-5F8306B0-7502-4A7E-A847-FBEAA137C28C";

		public WeatherService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		//https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWA-5F8306B0-7502-4A7E-A847-FBEAA137C28C&format=JSON&locationName=%E6%96%B0%E5%8C%97%E5%B8%82&elementName=CI&sort=time

		public async Task<WeatherData> GetWeatherForCityAsync(string city)
		{
			var encodedCity = Uri.EscapeDataString(city);
			var url = $"https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization={_apiKey}&format=JSON&locationName={encodedCity}&elementName=CI&sort=time";
			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();

			var weatherData = await JsonSerializer.DeserializeAsync<WeatherData>(
				await response.Content.ReadAsStreamAsync());

			if (weatherData == null)
			{
				throw new InvalidOperationException("Failed to deserialize weather data.");
			}

			return weatherData;
		}
	}
}
