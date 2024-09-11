using common;

namespace api.Services
{
    [UseCaseResolvable]
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> GetWeatherForecasts();
    }
}