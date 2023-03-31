using System.Net;
using Weather.Domain.Entities;
using Weather.Domain.Interfaces;
using Weather.Domain.Response;
using Weather.Domain.ViewModels;
using Weather.Services.Interfaces;

namespace Weather.Services.Implements
{
    public class ForecastUserService : IForecastUserService
    {
        private readonly IOpenWeatherService _openWeatherService;
        private readonly IForecastService _forecastService;

        public ForecastUserService(IForecastService forecastService, IOpenWeatherService openWeatherService)
        {
            _forecastService = forecastService;
            _openWeatherService = openWeatherService;
        }

        public async Task<IDefaultResponse<List<CityWeather>>> GetCities(int UId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return new DefaultResponse<List<CityWeather>>()
                {
                    Message = $"[GetCities] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<bool>> RemoveItem(int UId, int cityId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return new DefaultResponse<bool>()
                {
                    Message = $"[GetCities] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<CityWeather>> SearchByCityGeo(int UId, SearchForecastByGeoViewModel model)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return new DefaultResponse<CityWeather>()
                {
                    Message = $"[GetCities] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<CityWeather>> SearchByCityName(int UId, SearchForecastByNameViewModel model)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return new DefaultResponse<CityWeather>()
                {
                    Message = $"[GetCities] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }
    }
}
