using System.ComponentModel.DataAnnotations;

namespace Weather.Domain.ViewModels
{
    public class SearchForecastByNameViewModel
    {
        [MinLength(3, ErrorMessage = "Name must be minimum 3 chars.")]
        public string CityName { get; set; }
    }

    public class SearchForecastByGeoViewModel 
    {
        public float Lat { get; set; }

        public float Lon { get; set; }
    }
}
