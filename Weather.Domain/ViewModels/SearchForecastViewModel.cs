namespace Weather.Domain.ViewModels
{
    public class SearchForecastByNameViewModel
    {
        public string CityName { get; set; }
    }

    public class SearchForecastByGeoViewModel 
    {
        public float Lat { get; set; }

        public float Lon { get; set; }
    }
}
