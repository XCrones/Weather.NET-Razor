using Weather.Domain.Interfaces;

namespace Weather.Services.Interfaces
{
    public interface IGetAllUId<TD>
    {
        Task<IDefaultResponse<List<TD>>> GetAll(int UId);
    }
}
