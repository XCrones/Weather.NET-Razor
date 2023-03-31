using Weather.Domain.Interfaces;
using Weather.Domain.ViewModels;

namespace Weather.Services.Interfaces
{
    public interface IOpenWeatherService
    {
        public Task<IDefaultResponse<ForecastViewModel>> FetchByName(SearchForecastByNameViewModel model, string _apiKey);

        public Task<IDefaultResponse<ForecastViewModel>> FetchByGeo(SearchForecastByGeoViewModel model, string _apiKey);
    }
}
