using Microsoft.EntityFrameworkCore;
using System.Net;
using Weather.DAL.Interfaces;
using Weather.Domain.Entities;
using Weather.Domain.Helpers;
using Weather.Domain.Interfaces;
using Weather.Domain.Messages;
using Weather.Domain.Response;
using Weather.Domain.ViewModels;
using Weather.Services.Interfaces;

namespace Weather.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        private async Task<User?> SearchUniqEmail(string Email)
        {
            return await _repository.Read().FirstOrDefaultAsync(item => item.Email == Email);
        }

        private async Task<User?> SearchByUId(int UId)
        {
            return await _repository.Read().FirstOrDefaultAsync(item => item.UId == UId && item.DeleteAt == null);
        }

        public async Task<IDefaultResponse<User>> CreateItem(SignupViewModel model)
        {
            try
            {
                DateTime timeStamp = DateTime.UtcNow;

                var uniqEmail = await SearchUniqEmail(model.Email);

                if (uniqEmail != null)
                {
                    return new DefaultResponse<User>()
                    {
                        Message = DefaultMessage.EmailIsBusy,
                        HttpCode = HttpStatusCode.BadRequest,
                    };
                }

                var newItem = new User()
                {
                    Email = model.Email,
                    Name = model.Name,
                    CreatedAt = timeStamp,
                    UpdatedAt = timeStamp,
                    Password = PasswordHelper.HashPassword(model.Password),
                };

                await _repository.Create(newItem);

                return new DefaultResponse<User>()
                {
                    Message = DefaultMessage.CreateSucces,
                    HttpCode = HttpStatusCode.Created,
                    Data = newItem,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<User>()
                {
                    Message = $"[CreateItem] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<User>> GetItem(int UId)
        {
            try
            {
                var itemResponse = await SearchByUId(UId);

                if (itemResponse == null)
                {
                    return new DefaultResponse<User>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.NotFound,
                    };
                }

                return new DefaultResponse<User>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Data = itemResponse,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<User>()
                {
                    Message = $"[GetItem] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<bool>> RemoveItem(int UId)
        {
            try
            {
                var itemResponse = await SearchByUId(UId);

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

        public async Task<IDefaultResponse<User>> RestoreItem(int UId)
        {
            try
            {
                var itemResponse = await _repository.Read().FirstOrDefaultAsync(item => item.UId == UId && item.DeleteAt != null);

                if (itemResponse == null)
                {
                    return new DefaultResponse<User>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.NotFound,
                    };
                }

                itemResponse.DeleteAt = null;
                await _repository.Update(itemResponse);

                return new DefaultResponse<User>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Message = DefaultMessage.RestoreSucces,
                    Data = itemResponse,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<User>()
                {
                    Message = $"[RestoreItem] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<User>> UpdateItem(int UId, SignupViewModel model)
        {
            try
            {
                var itemResponse = await SearchByUId(UId);

                if (itemResponse == null)
                {
                    return new DefaultResponse<User>()
                    {
                        HttpCode = HttpStatusCode.NotFound,
                        Message = DefaultMessage.NotFound,
                    };
                }

                if (itemResponse.Email != model.Email)
                {
                    var uniqEmail = await SearchUniqEmail(model.Email);

                    if (uniqEmail != null)
                    {
                        return new DefaultResponse<User>()
                        {
                            Message = DefaultMessage.EmailIsBusy,
                            HttpCode = HttpStatusCode.BadRequest,
                        };
                    }
                }

                itemResponse.Email = model.Email;
                itemResponse.Name = model.Name;
                itemResponse.Password = PasswordHelper.HashPassword(model.Password);
                itemResponse.UpdatedAt = DateTime.UtcNow;

                var responseUpdate = await _repository.Update(itemResponse);

                if (responseUpdate == null)
                {
                    return new DefaultResponse<User>()
                    {
                        HttpCode = HttpStatusCode.InternalServerError,
                        Message = DefaultMessage.ErrorUpdate,
                    };
                }

                return new DefaultResponse<User>()
                {
                    HttpCode = HttpStatusCode.OK,
                    Data = responseUpdate,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<User>()
                {
                    Message = $"[UpdateItem] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IDefaultResponse<User>> SignIn(SigninViewModel model)
        {
            try
            {
                var itemResponse = await _repository.Read().FirstOrDefaultAsync(item => item.Email == model.Email && item.DeleteAt == null);

                if (itemResponse != null)
                {
                    var pass = PasswordHelper.HashPassword(model.Password);

                    if (pass == itemResponse.Password)
                    {
                        return new DefaultResponse<User>()
                        {
                            HttpCode = HttpStatusCode.OK,
                            Data = itemResponse,
                        };
                    }
                }

                return new DefaultResponse<User>()
                {
                    HttpCode = HttpStatusCode.BadRequest,
                    Message = DefaultMessage.IncorrectEmilOrPassword,
                };
            }
            catch (Exception ex)
            {
                return new DefaultResponse<User>()
                {
                    Message = $"[SignIn] : {ex.Message}",
                    HttpCode = HttpStatusCode.InternalServerError,
                };
            }
        }
    }
}
