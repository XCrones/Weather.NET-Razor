using Weather.Domain.Entities;
using Weather.Domain.Interfaces;
using Weather.Domain.ViewModels;

namespace Weather.Services.Interfaces
{
    public interface IForecastService : IBaseService<Forecast, ForecastViewModel>
    {
        Task<IDefaultResponse<Forecast>> GetItemByCityName(SearchForecastByNameViewModel model);

        Task<IDefaultResponse<Forecast>> GetItemByCityCoord(SearchForecastByGeoViewModel model);

        Task<IDefaultResponse<Forecast>> GetItemByCityId(int cityId);

    }
}
