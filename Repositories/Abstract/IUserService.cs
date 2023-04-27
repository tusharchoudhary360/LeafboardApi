﻿using AuthApi.Models.Domain;
using AuthApi.Models.DTO;

namespace AuthApi.Repositories.Abstract
{
    public interface IUserService
    {
        Task<AllUsers?> GetSingleUser(string email);
        Task<string> AddImage(IFormFile imageFile, string email);
        List<UserImages> ShowImage(string email);
        Task<string> DeleteImage(int id, string email);
    }
}
