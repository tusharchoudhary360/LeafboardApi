using AuthApi.Models.Domain;
using AuthApi.Models.DTO;

namespace AuthApi.Repositories.Abstract
{
    public interface IAuthService
    {
        Task<Users> FindByEmailAsync(string email);
        Task<string> CreateAdminAsync(RegisterModel model);
        Task<string> CreateUserAsync(RegisterModel model);
        Task<bool> CheckPasswordAsync(Users users, string password);
        Task<string> ChangePasswordAsync(Users users, string password);
        Task<LoginResponse> Login(LoginModel model);
        Task<string> VerifyEmailAsync(string email);
        Task<string> CheckOtpAsync(VerifyModel model);
        Task<string> VerifyUserAsync(Users model);
        string validiatePassword(string password);
    }
}
