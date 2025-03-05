using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Domain.Models.User;
using ProjectManagement.Service.DTOs.User;
using ProjectManagement.Service.DTOs.UserForCreationDTO;

namespace ProjectManagement.Service.Interfaces.User
{
    public interface IUserService
    {
        ValueTask<bool> CreateUser(UserForCreationDTO @dto);
        ValueTask<bool> UpdateUser(UserForUpdateDTO @dto);
        ValueTask<LoginModel> Login(UserForLoginDTO @dto);
        ValueTask<PagedResult<UserModel>> GetAsync();
    }
}
