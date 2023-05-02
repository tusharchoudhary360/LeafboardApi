using AuthApi.Models;
using AuthApi.Models.Domain;
using AuthApi.Models.DTO;
using AuthApi.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AuthApi.Repositories.Domain
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext _context;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        public AuthService(DatabaseContext context, IFileService fileService,IEmailService emailService, ITokenService tokenService)
        {
            _context = context;
            _emailService = emailService;
            _fileService = fileService;
            _tokenService = tokenService;

        }
        public async Task<Users> FindByEmailAsync(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u=>u.Email ==email.ToLower());
            if (user == null) 
            {
                return null;
            }
            return user;
        }
        public async Task<bool> CheckPasswordAsync(Users users, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == users.Email.ToLower());
            if (user.Password == password)
            {
                return true;
            }
            return false;
        }

        public async Task<string> ChangePasswordAsync(Users users,string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == users.Email.ToLower());
            if (user != null)
            {
                user.Password = password;
                _context.SaveChanges();
                return "Password change success";
            }
            return null;
        }

        public async Task<string> CreateAdminAsync(RegisterModel model)
        {
            try
            {
                var fileResult = _fileService.SaveImage(model.ImageFile);
                string Image = null;
                if (fileResult.Item1 == 1)
                {
                    Image = fileResult.Item2;
                }
               
                var AdminInstance = new Users
                {
                    Name = model.Name,
                    Email = model.Email.ToLower(),
                    Role = Roles.Admin,
                    Password = model.Password,
                    IsVerified = false,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Country = model.Country,
                    Hobby = (Models.Domain.Hobby)model.Hobby,
                    ProfileImage = Image,
                };
                await _context.Users.AddAsync(AdminInstance);
                _context.SaveChanges();
                return "Admin Created";
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> CreateUserAsync(RegisterModel model)
        {
            try
            {
                var fileResult = _fileService.SaveImage(model.ImageFile);
                string Image = null;
                if (fileResult.Item1 == 1)
                {
                    Image = fileResult.Item2;
                }

                var UserInstance = new Users
                {
                    Name = model.Name,
                    Email = model.Email.ToLower(),
                    Role = Roles.User,
                    Password = model.Password,
                    IsVerified = false,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Country = model.Country,
                    Hobby = (Models.Domain.Hobby)model.Hobby,
                    ProfileImage = Image,
                };
                await _context.Users.AddAsync(UserInstance);
                _context.SaveChanges();

                var AllUserInstance = new AllUsers
                {
                    Name = model.Name,
                    Email = model.Email.ToLower(),
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Country = model.Country,
                    Hobby = model.Hobby,
                    ProfileImage = Image,
                };
                await _context.AllUsers.AddAsync(AllUserInstance);
                _context.SaveChanges();
                return "User Created";
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<LoginResponse> Login(LoginModel model)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email.ToLower());
                var token = _tokenService.GetToken(user, user.Role);
                var refreshToken = _tokenService.GetRefreshToken();
                var tokenInfo = _context.Token.FirstOrDefault(a => a.UserEmail == user.Email.ToLower());
                if (tokenInfo == null)
                {
                    var info = new Token
                    {
                        UserEmail = user.Email.ToLower(),
                        RefreshToken = refreshToken,
                        RefreshTokenExpiry = DateTime.Now.AddDays(1)
                    };
                    _context.Token.Add(info);
                }
                else
                {
                    tokenInfo.RefreshToken = refreshToken;
                    tokenInfo.RefreshTokenExpiry = DateTime.Now.AddDays(1);
                }
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return null;
                }
                var result = new LoginResponse
                {
                    Name = user.Name,
                    Email = user.Email.ToLower(),
                    Role = user.Role,
                    Token = token.TokenString,
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                };
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> VerifyEmailAsync(string email)
        {
            try
            {
                Random rnd = new Random();
                var otp = rnd.Next(100000, 999999);
                var user = await _context.Otp.FirstOrDefaultAsync(u => u.UserEmail == email.ToLower());
                if (user == null)
                {
                    Otp otpModel = new Otp { UserEmail = email.ToLower(), otp = otp, ExpiryTime = DateTime.Now.AddMinutes(5) };
                    await _context.Otp.AddAsync(otpModel);
                    _context.SaveChanges();
                }
                else
                {
                    user.otp = otp;
                    user.ExpiryTime = DateTime.Now.AddMinutes(5);
                    _context.SaveChanges();
                }
                Message message = new Message { To = email.ToLower(), Subject = "Please verify your email", Content = otp.ToString() };
                _emailService.SendEmail(message);
                return "Otp Send ";
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> CheckOtpAsync(VerifyModel model)
        {
            var OTP = await _context.Otp.FirstOrDefaultAsync(o=>o.UserEmail==model.Email.ToLower());
            if (OTP == null)
            {
                return "No record Found, Please Enter valid Email";
            }
           
            if (OTP.otp != model.Otp)
            {
                return "Invalid OTP";
            }
            if (OTP.ExpiryTime < DateTime.Now)
            {
                return "Otp Expired";
            }
            return null;
        }

        public async Task<string> VerifyUserAsync(Users model)
        {
            try
            {
                var result = await _context.Users.FirstOrDefaultAsync(o => o.Email == model.Email.ToLower());
                result.IsVerified = true;
                _context.SaveChanges();
                return "User Verified ";
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string validiatePassword(string password)
        {
            if (password.Length < 6)
                return "Password minimum length should be 6 character";

            if (!password.Any(char.IsUpper))
                return "Atleast One Upper Case Character required in password";

            if (!password.Any(char.IsDigit))
                return "Atleast One Numeric Character required in password";

            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";

            char[] specialChArray = specialCh.ToCharArray();

            foreach (char ch in specialChArray)
            {
                if (password.Contains(ch))
                    return null;
            }

            return "Atleast One special Character required in password";
        }
    }
}
