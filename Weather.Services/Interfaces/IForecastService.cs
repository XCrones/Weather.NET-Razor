using Weather.Domain.Entities;
using Weather.Domain.ViewModels;

namespace Weather.Services.Interfaces
{
    public interface IForecastService : IBaseService<Forecast, ForecastViewModel>
    {
    }
}
