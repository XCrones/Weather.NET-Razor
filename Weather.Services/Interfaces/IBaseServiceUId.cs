using Weather.Domain.Interfaces;

namespace Weather.Services.Interfaces
{
    public interface IBaseServiceUId<TD, VM>
    {
        Task<IDefaultResponse<TD>> CreateItem(int UId, VM model);

        Task<IDefaultResponse<TD>> UpdateItem(int UId, int id, VM model);

        Task<IDefaultResponse<TD>> GetItem(int UId, int id);

        Task<IDefaultResponse<bool>> RemoveItem(int UId, int id);

        Task<IDefaultResponse<TD>> RestoreItem(int UId, int id);
    }
}
