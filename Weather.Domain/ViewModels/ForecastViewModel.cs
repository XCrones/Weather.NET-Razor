using Weather.Domain.Entities;

namespace Weather.Domain.ViewModels
{
    public class ForecastViewModel
    {
        public string cod { get; set; }

        public int message { get; set; }

        public int cnt { get; set; }

        public List<WeatherItem> list { get; set; }

        public CityForecast city { get; set; }
    }
}
