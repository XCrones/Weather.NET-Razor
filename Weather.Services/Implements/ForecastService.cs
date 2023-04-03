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
    public class ForecastService : IForecastService
    {
        private readonly IForecastRepository _repository;

        public ForecastService(IForecastRepository forecastRepository)
        {
            _repository = forecastRepository;
        }

        private async Task<Forecast?> GetForecastById(int id)
        {
            return await _repository.Read().FirstOrDefaultAsync(x => x.Id == id && x.DeleteAt == null);
        }

        private async Task<Forecast?> SearchByNameCity(string cityName)
        {
            var subName = cityName.Substring(1).ToLower();
            var nameCapitalize = $"{char.ToUpper(cityName[0])}{subName}";
            return  await _repository.Read().FirstOrDefaultAsync(x => x.city.name == nameCapitalize.Trim() && x.DeleteAt == null);
        }

        public async Task<IDefaultResponse<Forecast>> CreateItem(ForecastViewModel model)
        {
            try
            {
                DateTime timeStamp = DateTime.UtcNow;

                Forecast newItem = new()
                {
                    list = model.list,
                    city = model.city,
                    CreatedAt = timeStamp,
                    UpdatedAt = timeStamp,
                };

                await _repository.Create(newItem);

                return new DefaultResponse<Forecast>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Data = newItem,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<Forecast>()
                {
                    Message = $"[CreateItem] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<Forecast>> GetItem(int id)
        {
            try
            {
                var itemResponse = await GetForecastById(id);

                if (itemResponse == null)
                {
                    return new DefaultResponse<Forecast>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.NotFound,
                    };
                }

                return new DefaultResponse<Forecast>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Data = itemResponse,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<Forecast>()
                {
                    Message = $"[GetItem] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<bool>> RemoveItem(int id)
        {
            try
            {
                var itemResponse = await GetForecastById(id);

                if (itemResponse == null)
                {
                    return new DefaultResponse<bool>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.NotFound,
                    };
                }

                itemResponse.DeleteAt = DateTime.UtcNow;
                await _repository.Update(itemResponse);

                return new DefaultResponse<bool>()
                {
                    Message = DefaultMessage.RemoveSucces,
                    HttpCode = HttpStatusCode.OK,
                    Data = true,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<bool>()
                {
                    Message = $"[RemoveItem] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<Forecast>> RestoreItem(int id)
        {
            try
            {
                var itemResponse = await _repository.Read().FirstOrDefaultAsync(x => x.DeleteAt != null);

                if (itemResponse == null)
                {
                    return new DefaultResponse<Forecast>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.NotFound,
                    };
                }

                itemResponse.DeleteAt = null;
                await _repository.Update(itemResponse);

                return new DefaultResponse<Forecast>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Message = DefaultMessage.RestoreSucces,
                    Data = itemResponse,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<Forecast>()
                {
                    Message = $"[RestoreItem] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<Forecast>> UpdateItem(int id, ForecastViewModel model)
        {
            try
            {
                var itemResponse = await GetForecastById(id);

                if (itemResponse == null)
                {
                    return new DefaultResponse<Forecast>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.NotFound,
                    };
                }

                itemResponse.UpdatedAt = DateTime.UtcNow;
                itemResponse.city = model.city;
                itemResponse.list = model.list;

                var responseUpdate = await _repository.Update(itemResponse);

                if (responseUpdate == null)
                {
                    return new DefaultResponse<Forecast>()
                    {
                        HttpCode = HttpStatusCode.InternalServerError,
                        Message = DefaultMessage.ErrorUpdate,
                    };
                }

                return new DefaultResponse<Forecast>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Data = responseUpdate,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<Forecast>()
                {
                    Message = $"[UpdateItem] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<Forecast>> GetItemByCityName(SearchForecastByNameViewModel model)
        {
            try
            {
                var itemResponse = await SearchByNameCity(model.CityName);

                if (itemResponse == null)
                {
                    return new DefaultResponse<Forecast>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.NotFound,
                    };
                }

                return new DefaultResponse<Forecast>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Data = itemResponse,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<Forecast>()
                {
                    Message = $"[GetItemByCityName] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<Forecast>> GetItemByCityCoord(SearchForecastByGeoViewModel model)
        {
            try
            {
                var itemResponse = await _repository.Read().FirstOrDefaultAsync(x => x.city.coord.lat == model.Lat && x.city.coord.lon == model.Lon && x.DeleteAt == null);

                if (itemResponse == null)
                {
                    return new DefaultResponse<Forecast>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.NotFound,
                    };
                }

                return new DefaultResponse<Forecast>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Data = itemResponse,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<Forecast>()
                {
                    Message = $"[GetItemByCityCoord] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<Forecast>> GetItemByCityId(int cityId)
        {
            try
            {
                var itemResponse = await _repository.Read().FirstOrDefaultAsync(x => x.city.id == cityId && x.DeleteAt == null);

                if (itemResponse == null)
                {
                    return new DefaultResponse<Forecast>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.NotFound,
                    };
                }

                return new DefaultResponse<Forecast>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Data = itemResponse,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<Forecast>()
                {
                    Message = $"[GetItemByCityId] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }
    }
}
