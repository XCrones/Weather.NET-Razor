using System.Net;
using Weather.Domain.Interfaces;
using Weather.Domain.Response;
using Weather.Domain.ViewModels;
using Weather.Services.Interfaces;

namespace Weather.Services.Implements
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _apiKey = "YOUR_API_KEY";
        private readonly string _urlApi = "https://api.openweathermap.org/data/2.5";

        public OpenWeatherService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        private async Task<ForecastViewModel?> FetchForecast(string queries)
        {
            var result = await _httpClientService.Get<ForecastViewModel?>($"{_urlApi}/forecast/?{queries}&units=metric&appid={_apiKey}");
            return result;
        }

        public async Task<IDefaultResponse<ForecastViewModel>> FetchByGeo(SearchForecastByGeoViewModel model)
        {
            try
            {
                var response = await FetchForecast($"lat={model.Lat}&lon={model.Lon}");

                if (response != null && response.cod == "200")
                {
                    return new DefaultResponse<ForecastViewModel>()
                    {
                        HttpCode = HttpStatusCode.OK,
                        Data = response
                    };
                }

                return new DefaultResponse<ForecastViewModel>()
                {
                    HttpCode = HttpStatusCode.BadRequest,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<ForecastViewModel>()
                {
                    HttpCode = HttpStatusCode.InternalServerError,
                    Message = $"[FetchByGeo] : {ex.Message}",
                };
            }
        }

        public async Task<IDefaultResponse<ForecastViewModel>> FetchByName(SearchForecastByNameViewModel model)
        {
            try
            {
                var response = await FetchForecast($"q={model.CityName}");

                if (response != null && response.cod == "200")
                {
                    return new DefaultResponse<ForecastViewModel>()
                    {
                        HttpCode = HttpStatusCode.OK,
                        Data = response
                    };
                }

                return new DefaultResponse<ForecastViewModel>()
                {
                    HttpCode = HttpStatusCode.BadRequest,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<ForecastViewModel>()
                {
                    Message = $"[FetchByName] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }
    }
}
