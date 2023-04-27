using AuthApi.Models.Domain;
using AuthApi.Models.DTO;
using AuthApi.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthorizationController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Status(400, "Please pass all the Fields", null));
            }
            var checkpass = _authService.validiatePassword(model.Password);
            if (checkpass != null)
            {
                return Ok(new Status(400, checkpass, null));
            }
            var emailExist = await _authService.FindByEmailAsync(model.Email);
            if (emailExist != null)
            {
                return Ok(new Status(400, "Email already register", null));
            }
            var result = await _authService.CreateAdminAsync(model);
            if (result == null)
            {
                return Ok(new Status(400, "Admin creation failed", null));
            }
            var send = await _authService.VerifyEmailAsync(model.Email);
            return Ok(new Status(200, "Otp Send to Your Email, Please verify your email", null));
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Status(400, "Please pass all the Fields", null));
            }
            var checkpass = _authService.validiatePassword(model.Password);
            if (checkpass != null)
            {
                return Ok(new Status(400, checkpass, null));
            }
            var emailExist = await _authService.FindByEmailAsync(model.Email);
            if (emailExist != null)
            {
                return Ok(new Status(400, "Email already register", null));
            }
            var result = await _authService.CreateUserAsync(model);
            if (result == null)
            {
                return Ok(new Status(400, "User creation failed", null));
            }
            var send = await _authService.VerifyEmailAsync(model.Email);
            return Ok(new Status(200, "Otp Send to Your Email, Please verify your email", null));
        }

        [HttpPost]
        public async Task<IActionResult> ResendOtp(JustEmail model)
        {
            if (model.Email == null)
            {
                return Ok(new Status(400, "please Enter your Email", null));
            }
            var emailExist = await _authService.FindByEmailAsync(model.Email);
            if (emailExist == null)
            {
                return Ok(new Status(400, "User not Exist, Please register first", null));
            }
            var send = await _authService.VerifyEmailAsync(emailExist.Email);
            return Ok(new Status(200, "Otp Send to Your Email, Please verify your email", null));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model) //Not forgotPassword
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Status(400, "please pass all the valid fields", null));
            }
            var checkpass = _authService.validiatePassword(model.NewPassword);
            if (checkpass != null)
            {
                return Ok(new Status(400, checkpass, null));
            }
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                return Ok(new Status(400, "New Password and Confirm password should be same.", null));
            }
            var user = await _authService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new Status(400, "Invalid Email", null));
            }
            if (!await _authService.CheckPasswordAsync(user, model.CurrentPassword))
            {
                return Ok(new Status(400, "invalid current password", null));
            }
            var result = await _authService.ChangePasswordAsync(user, model.NewPassword);
            if (result == null)
            {
                return Ok(new Status(400, "Change Password failed", null));
            }
            return Ok(new Status(200, "Password has changed successfully", null));
        }

        [HttpPost]
        public async Task<IActionResult>VerifyRegistration(VerifyModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Status(400, "please pass all the valid fields", null));
            }
            var user = await _authService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new Status(400, "Invalid Email", null));
            }
            var result = await _authService.CheckOtpAsync(model);
            if (result != null)
            {
                return Ok(new Status(400, result, null));
            }
            var res = await _authService.VerifyUserAsync(user);
            if (res == null)
            {
                return Ok(new Status(400, "Server Error", null));
            }
            return Ok(new Status(400, "Registration Successfull", null));
           
        }

        [HttpPost]
        public async Task<IActionResult> forgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Status(400, "please pass all the valid fields", null));
            }
            var checkpass = _authService.validiatePassword(model.NewPassword);
            if (checkpass != null)
            {
                return Ok(new Status(400, checkpass, null));
            }
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                return Ok(new Status(400, "New Password and Confirm password should be same.", null));
            }
            var user = await _authService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new Status(400, "Invalid Email", null));
            }
            VerifyModel otpInstance = new VerifyModel
            {
                Email=model.Email,
                Otp=model.Otp
            };
            var result = await _authService.CheckOtpAsync(otpInstance);
            if (result != null)
            {
                return Ok(new Status(400, result, null));
            }
            var res = await _authService.ChangePasswordAsync(user, model.NewPassword);
            if (res == null)
            {
                return Ok(new Status(400, "Change Password failed", null));
            }
            return Ok(new Status(200, "Password has changed successfully", null));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Status(400, "please pass all the valid fields", null));
            }
            var user = await _authService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new Status(400, "User not Exist", null));
            }
            if (!await _authService.CheckPasswordAsync(user, model.Password))
            {
                return Ok(new Status(400, "incorrect password", null));
            }
            if (!user.IsVerified)
            {
                var send = await _authService.VerifyEmailAsync(user.Email);
                return Ok(new Status(202, "Otp Send to Your Email, Please verify your email", null));
            }
            var result = await _authService.Login(model);
            if (result == null)
            {
                return Ok(new Status(400, "Login failed", null));
            }
            return Ok(new Status(200, "Login success", result)); ;
        }
    }
}
