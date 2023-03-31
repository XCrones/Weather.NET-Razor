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

        public ForecastService(IForecastRepository forecastRepository, IOpenWeatherService openWeatherService)
        {
            _repository = forecastRepository;
        }

        private async Task<Forecast?> GetForecastById(int id)
        {
            return await _repository.Read().FirstOrDefaultAsync(item => item.city.id == id && item.DeleteAt != null);
        }

        public async Task<IDefaultResponse<Forecast>> CreateItem(ForecastViewModel model)
        {
            try
            {
                DateTime timeStamp = DateTime.UtcNow;

                Forecast newItem = new Forecast()
                {
                    cnt = model.cnt,
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
                var itemResponse = await _repository.Read().FirstOrDefaultAsync(item => item.DeleteAt != null);

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
                itemResponse.cnt = model.cnt;
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
    }
}
