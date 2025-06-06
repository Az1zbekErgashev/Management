﻿using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Domain.Models.User;
using ProjectManagement.Service.DTOs.User;
using ProjectManagement.Service.DTOs.UserForCreationDTO;
using System.Security.Claims;

namespace ProjectManagement.Service.Interfaces.User
{
    public interface IUserService
    {
        ValueTask<bool> CreateUser(UserForCreationDTO @dto);
        ValueTask<bool> UpdateUser(UserForUpdateDTO @dto);
        ValueTask<LoginModel> Login(UserForLoginDTO @dto);
        ValueTask<PagedResult<UserModel>> GetAsync(UserForFilterDTO @dto);
        ValueTask<UserModel> GetByIdAsync(int userId);
        public string TokenGenerator(IEnumerable<Claim> claims);
        ValueTask<string> DeleteUser(int userId, bool isHardDelete);
        ValueTask<List<UserEmailsModel>> GetUserEmails();
        ValueTask<UserModel> GetProfile();
    }
}
