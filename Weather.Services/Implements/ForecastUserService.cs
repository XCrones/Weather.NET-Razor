using Microsoft.EntityFrameworkCore;
using System.Net;
using Weather.DAL.Interfaces;
using Weather.Domain.Entities;
using Weather.Domain.Interfaces;
using Weather.Domain.Messages;
using Weather.Domain.Response;
using Weather.Domain.ViewModels;
using Weather.Services.Interfaces;

namespace Weather.Services.Implements
{
    public class ForecastUserService : IForecastUserService
    {
        private readonly IOpenWeatherService _openWeatherService;
        private readonly IForecastService _forecastService;
        private readonly IForecastUserRepository _repository;

        public ForecastUserService(IForecastService forecastService, IOpenWeatherService openWeatherService, IForecastUserRepository repository)
        {
            _forecastService = forecastService;
            _openWeatherService = openWeatherService;
            _repository = repository;
        }

        private async Task<ForecastUser?> GetForecastUser(int UId)
        {
            return await _repository.Read().FirstOrDefaultAsync(x => x.UId == UId);
        }

        private async Task<IDefaultResponse<CityWeather>> CreateOrUpdateForecast(IDefaultResponse<ForecastViewModel>? fetchForecast)
        {
            try
            {
                if (fetchForecast == null || fetchForecast.HttpCode != HttpStatusCode.OK || fetchForecast.Data == null)
                {
                    return new DefaultResponse<CityWeather>()
                    {
                        HttpCode = HttpStatusCode.BadRequest,
                        Message = DefaultMessage.CityNotFound,
                    };
                }


                var uniqForecast = await _forecastService.GetItemByCityId(fetchForecast.Data.city.id);

                if (uniqForecast.HttpCode == HttpStatusCode.OK && uniqForecast.Data != null)
                {
                    var itemUpdate = await _forecastService.UpdateItem(uniqForecast.Data.Id, fetchForecast.Data);

                    if (itemUpdate.HttpCode == HttpStatusCode.OK && itemUpdate.Data != null)
                    {
                        return new DefaultResponse<CityWeather>()
                        {
                            HttpCode = HttpStatusCode.OK,
                            Message = DefaultMessage.UpdateSucces,
                            Data = new CityWeather()
                            {
                                country = itemUpdate.Data.city.country,
                                name = itemUpdate.Data.city.name,
                                id = itemUpdate.Data.Id,
                            }
                        };
                    }
                }

                var responseCreate = await _forecastService.CreateItem(fetchForecast.Data);

                if (responseCreate.HttpCode != HttpStatusCode.OK || responseCreate.Data == null)
                {
                    return new DefaultResponse<CityWeather>()
                    {
                        HttpCode = HttpStatusCode.InternalServerError,
                        Message = DefaultMessage.ErrorSave
                    };
                }

                return new DefaultResponse<CityWeather>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Message = DefaultMessage.CreateSucces,
                    Data = new CityWeather()
                    {
                        country = responseCreate.Data.city.country,
                        name = responseCreate.Data.city.name,
                        id = responseCreate.Data.Id
                    }
                };
            } catch (Exception ex)
            {
                return new DefaultResponse<CityWeather>()
                {
                    Message = $"[GetCities] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<List<CityWeather>>> GetCities(int UId)
        {
            try
            {
                var itemResponse = await GetForecastUser(UId);

                if (itemResponse == null)
                {
                    DateTime timeStamp = DateTime.UtcNow;

                    var newItem = new ForecastUser() 
                    {
                        Cities = new List<CityWeather>(),
                        CreatedAt = timeStamp,
                        UpdatedAt = timeStamp,
                    };

                    await _repository.Create(newItem);

                    return new DefaultResponse<List<CityWeather>> ()
                    {
                        HttpCode = HttpStatusCode.OK,
                        Data = newItem.Cities,
                    };
                }

                return new DefaultResponse<List<CityWeather>>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Data = itemResponse.Cities,
                };
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
                var itemResponse = await GetForecastUser(UId);

                if (itemResponse == null)
                {
                    return new DefaultResponse<bool>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.UserNotFound,
                    };
                }

                var searchCity = itemResponse.Cities.Where(item => item.id != cityId).ToList();

                if (searchCity == null)
                {
                    return new DefaultResponse<bool>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.NotFound,
                    };
                }

                itemResponse.Cities = itemResponse.Cities.Where(x => x.id != cityId).ToList();
                itemResponse.UpdatedAt = DateTime.UtcNow;

                await _repository.Update(itemResponse);

                return new DefaultResponse<bool>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Message = DefaultMessage.RemoveSucces,
                };
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

        public async Task<IDefaultResponse<CityWeather>> SearchByCityCoord(int UId, SearchForecastByGeoViewModel model)
        {
            try
            {
                var itemResponse = await _forecastService.GetItemByCityCoord(model);

                if (itemResponse.HttpCode == HttpStatusCode.InternalServerError)
                {
                    return new DefaultResponse<CityWeather>()
                    {
                        HttpCode = HttpStatusCode.InternalServerError,
                        Message = itemResponse.Message,
                    };
                }

                if (itemResponse.HttpCode == HttpStatusCode.OK && itemResponse.Data != null)
                {
                    if (itemResponse.Data.UpdatedAt.AddHours(1) > DateTime.UtcNow)
                    {
                        return new DefaultResponse<CityWeather>()
                        {
                            HttpCode = HttpStatusCode.OK,
                            Message = DefaultMessage.NoNeedToUpdate,
                            Data = new CityWeather()
                            {
                                country = itemResponse.Data.city.country,
                                name = itemResponse.Data.city.name,
                                id = itemResponse.Data.Id,
                            }
                        };
                    }
                }

                var fetchForecast = await _openWeatherService.FetchByGeo(model);
                return await CreateOrUpdateForecast(fetchForecast);
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
                var itemResponse = await _forecastService.GetItemByCityName(model);

                if (itemResponse.HttpCode == HttpStatusCode.InternalServerError)
                {
                    return new DefaultResponse<CityWeather>()
                    {
                        HttpCode = HttpStatusCode.InternalServerError,
                        Message = itemResponse.Message,
                    };
                }

                if (itemResponse.HttpCode == HttpStatusCode.OK && itemResponse.Data != null)
                {
                    if (itemResponse.Data.UpdatedAt.AddHours(1) > DateTime.UtcNow)
                    {
                        return new DefaultResponse<CityWeather>()
                        {
                            HttpCode = HttpStatusCode.OK,
                            Message = DefaultMessage.NoNeedToUpdate,
                            Data = new CityWeather()
                            {
                                country = itemResponse.Data.city.country,
                                name = itemResponse.Data.city.name,
                                id = itemResponse.Data.Id,
                            }
                        };
                    }
                }

                var fetchForecast = await _openWeatherService.FetchByName(model);
                return await CreateOrUpdateForecast(fetchForecast);
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
