using Weather.Domain.Entities;
using Weather.Domain.Interfaces;
using Weather.Domain.ViewModels;

namespace Weather.Services.Interfaces
{
    public interface IUserService : IBaseService<User, SignupViewModel>
    {
        Task<IDefaultResponse<User>> SignIn(SigninViewModel model);
    }
}
