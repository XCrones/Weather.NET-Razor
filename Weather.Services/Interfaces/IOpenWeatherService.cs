using Weather.Domain.Interfaces;
using Weather.Domain.ViewModels;

namespace Weather.Services.Interfaces
{
    public interface IOpenWeatherService
    {
        public Task<IDefaultResponse<ForecastViewModel>> FetchByName(SearchForecastByNameViewModel model);

        public Task<IDefaultResponse<ForecastViewModel>> FetchByGeo(SearchForecastByGeoViewModel model);
    }
}
