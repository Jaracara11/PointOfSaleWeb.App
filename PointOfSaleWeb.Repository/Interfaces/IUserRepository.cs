﻿using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<DbResponse<UserInfoDTO>> GetUserData(UserLoginDTO user);
        Task<DbResponse<UserInfoDTO>> CreateUser(User user);
        Task<DbResponse<UserInfoDTO>> UpdateUser(UserUpdateDTO user);
        Task<DbResponse<UserInfoDTO>> DeleteUser(int id);
    }
}
