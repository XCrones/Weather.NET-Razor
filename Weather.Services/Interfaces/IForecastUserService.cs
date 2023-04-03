using Weather.Domain.Entities;
using Weather.Domain.Interfaces;
using Weather.Domain.ViewModels;

namespace Weather.Services.Interfaces
{
    public interface IForecastUserService
    {
        public Task<IDefaultResponse<List<CityWeather>>> GetCities(int UId);

        public Task<IDefaultResponse<bool>> RemoveItem(int UId, int cityId);

        public Task<IDefaultResponse<List<CityWeather>>> SearchByCityName(int UId, SearchForecastByNameViewModel model, string _apiKey);

        public Task<IDefaultResponse<List<CityWeather>>> SearchByCityCoord(int UId, SearchForecastByGeoViewModel model,  string _apiKey);
    }
}
