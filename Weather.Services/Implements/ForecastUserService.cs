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

        private async Task<ForecastUser?> GetListCities(int UId)
        {
            return await _repository.Read().FirstOrDefaultAsync(x => x.UId == UId);
        }

        private async Task<IDefaultResponse<CityWeather>> CreateOrUpdateCitiesList(int UId, CityWeather model) 
        {
            try
            {
                var itemResponse = await GetListCities(UId);

                if (itemResponse == null)
                {
                    DateTime timeStamp = DateTime.UtcNow;

                    ForecastUser newItem = new()
                    {
                        UId = UId,
                        Cities = new List<CityWeather>(),
                        CreatedAt = timeStamp,
                        UpdatedAt = timeStamp,
                    };

                    newItem.Cities.Add(model);
                    await _repository.Create(newItem);

                    return new DefaultResponse<CityWeather>()
                    {
                        Data = new CityWeather()
                        {
                            id = model.id,
                            country = model.country,
                            name = model.name
                        },
                        HttpCode = HttpStatusCode.Created,
                        Message = DefaultMessage.CreateSucces,
                    };
                }

                var searchUniqCity = itemResponse.Cities.FirstOrDefault(x => x.id == model.id);

                if (searchUniqCity != null)
                {
                    return new DefaultResponse<CityWeather>()
                    {
                        Data = new CityWeather()
                        {
                            id = model.id,
                            country = model.country,
                            name = model.name
                        },
                        HttpCode = HttpStatusCode.OK,
                        Message = DefaultMessage.NoNeedToUpdate,
                    };
                }

                itemResponse.Cities.Add(model);
                itemResponse.UpdatedAt = DateTime.UtcNow;

                await _repository.Update(itemResponse);

                return new DefaultResponse<CityWeather>()
                {
                    Data = new CityWeather()
                    {
                        id = model.id,
                        country = model.country,
                        name = model.name
                    },
                    HttpCode = HttpStatusCode.OK,
                    Message = DefaultMessage.UpdateSucces,
                };
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

        private async Task<IDefaultResponse<CityWeather>> CreateOrUpdateForecast(int UId, ForecastViewModel model)
        {
            try
            {
                var uniqForecast = await _forecastService.GetItemByCityId(model.city.id);

                if (uniqForecast.HttpCode == HttpStatusCode.OK && uniqForecast.Data != null)
                {
                    var itemUpdate = await _forecastService.UpdateItem(uniqForecast.Data.Id, model);

                    if (itemUpdate.HttpCode == HttpStatusCode.OK && itemUpdate.Data != null)
                    {
                        var itemCityList = new CityWeather()
                        {
                            country = itemUpdate.Data.city.country,
                            name = itemUpdate.Data.city.name,
                            id = itemUpdate.Data.Id
                        };

                        return await CreateOrUpdateCitiesList(UId, itemCityList);
                    }
                }

                var responseCreate = await _forecastService.CreateItem(model);

                if (responseCreate.HttpCode != HttpStatusCode.OK || responseCreate.Data == null)
                {
                    return new DefaultResponse<CityWeather>()
                    {
                        HttpCode = HttpStatusCode.InternalServerError,
                        Message = DefaultMessage.ErrorSave
                    };
                }

                var newItem = new CityWeather()
                {
                    country = responseCreate.Data.city.country,
                    name = responseCreate.Data.city.name,
                    id = responseCreate.Data.Id
                };

                return await CreateOrUpdateCitiesList(UId, newItem);
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
                var itemResponse = await GetListCities(UId);

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
                var itemResponse = await GetListCities(UId);

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

        public async Task<IDefaultResponse<CityWeather>> SearchByCityCoord(int UId, SearchForecastByGeoViewModel model, string _apiKey)
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
                        var itemCityList = new CityWeather()
                        {
                            country = itemResponse.Data.city.country,
                            name = itemResponse.Data.city.name,
                            id = itemResponse.Data.Id
                        };

                        return await CreateOrUpdateCitiesList(UId, itemCityList);
                    }
                }

                var fetchForecast = await _openWeatherService.FetchByGeo(model, _apiKey);

                if (fetchForecast == null || fetchForecast.HttpCode != HttpStatusCode.OK || fetchForecast.Data == null)
                {
                    return new DefaultResponse<CityWeather>()
                    {
                        HttpCode = HttpStatusCode.BadRequest,
                        Message = DefaultMessage.CityNotFound,
                    };
                }

                return await CreateOrUpdateForecast(UId, fetchForecast.Data);
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

        public async Task<IDefaultResponse<CityWeather>> SearchByCityName(int UId, SearchForecastByNameViewModel model, string _apiKey)
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
                        var itemCityList = new CityWeather()
                        {
                            country = itemResponse.Data.city.country,
                            name = itemResponse.Data.city.name,
                            id = itemResponse.Data.Id
                        };

                        return await CreateOrUpdateCitiesList(UId, itemCityList);
                    }
                }

                var fetchForecast = await _openWeatherService.FetchByName(model, _apiKey);

                if (fetchForecast == null || fetchForecast.HttpCode != HttpStatusCode.OK || fetchForecast.Data == null)
                {
                    return new DefaultResponse<CityWeather>()
                    {
                        HttpCode = HttpStatusCode.BadRequest,
                        Message = DefaultMessage.CityNotFound,
                    };
                }

                return await CreateOrUpdateForecast(UId, fetchForecast.Data);
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
