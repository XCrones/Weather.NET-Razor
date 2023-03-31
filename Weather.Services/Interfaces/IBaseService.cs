using Weather.Domain.Interfaces;

namespace Weather.Services.Interfaces
{
    public interface IBaseService<TD, VM>
    {
        Task<IDefaultResponse<TD>> CreateItem(VM model);

        Task<IDefaultResponse<TD>> UpdateItem(int id, VM model);

        Task<IDefaultResponse<TD>> GetItem(int id);

        Task<IDefaultResponse<bool>> RemoveItem(int id);

        Task<IDefaultResponse<TD>> RestoreItem(int id);
    }
}
