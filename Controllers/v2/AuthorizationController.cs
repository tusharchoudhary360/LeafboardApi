using AuthApi.Models.Domain;
using AuthApi.Models.DTO;
using AuthApi.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
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
                return Ok(new StatusNew(400, "Please pass all the Fields"));
            }
            var checkpass = _authService.validiatePassword(model.Password);
            if (checkpass != null)
            {
                return Ok(new StatusNew(400, checkpass));
            }
            var emailExist = await _authService.FindByEmailAsync(model.Email);
            if (emailExist != null)
            {
                return Ok(new StatusNew(400, "Email already register"));
            }
            var result = await _authService.CreateAdminAsync(model);
            if (result == null)
            {
                return Ok(new StatusNew(400, "Admin creation failed"));
            }
            var send = await _authService.VerifyEmailAsync(model.Email);
            return Ok(new StatusNew(200, "Otp Send to Your Email, Please verify your email"));
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new StatusNew(400, "Please pass all the Fields"));
            }
            var checkpass = _authService.validiatePassword(model.Password);
            if (checkpass != null)
            {
                return Ok(new StatusNew(400, checkpass));
            }
            var emailExist = await _authService.FindByEmailAsync(model.Email);
            if (emailExist != null)
            {
                return Ok(new StatusNew(400, "Email already register"));
            }
            var result = await _authService.CreateUserAsync(model);
            if (result == null)
            {
                return Ok(new StatusNew(400, "User creation failed"));
            }
            var send = await _authService.VerifyEmailAsync(model.Email);
            return Ok(new StatusNew(200, "Otp Send to Your Email, Please verify your email"));
        }

        [HttpPost]
        public async Task<IActionResult> ResendOtp(JustEmail model)
        {
            if (model.Email == null)
            {
                return Ok(new StatusNew(400, "please Enter your Email"));
            }
            var emailExist = await _authService.FindByEmailAsync(model.Email);
            if (emailExist == null)
            {
                return Ok(new StatusNew(400, "User not Exist, Please register first"));
            }
            var send = await _authService.VerifyEmailAsync(emailExist.Email);
            return Ok(new StatusNew(200, "Otp Send to Your Email, Please verify your email"));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model) //Not forgotPassword
        {
            if (!ModelState.IsValid)
            {
                return Ok(new StatusNew(400, "please pass all the valid fields"));
            }
            var checkpass = _authService.validiatePassword(model.NewPassword);
            if (checkpass != null)
            {
                return Ok(new StatusNew(400, checkpass));
            }
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                return Ok(new StatusNew(400, "New Password and Confirm password should be same."));
            }
            var user = await _authService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new StatusNew(400, "Invalid Email"));
            }
            if (!await _authService.CheckPasswordAsync(user, model.CurrentPassword))
            {
                return Ok(new StatusNew(400, "invalid current password"));
            }
            var result = await _authService.ChangePasswordAsync(user, model.NewPassword);
            if (result == null)
            {
                return Ok(new StatusNew(400, "Change Password failed"));
            }
            return Ok(new StatusNew(200, "Password has changed successfully"));
        }

        [HttpPost]
        public async Task<IActionResult>VerifyRegistration(VerifyModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new StatusNew(400, "please pass all the valid fields"));
            }
            var user = await _authService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new StatusNew(400, "Invalid Email"));
            }
            var result = await _authService.CheckOtpAsync(model);
            if (result != null)
            {
                return Ok(new StatusNew(400, result));
            }
            var res = await _authService.VerifyUserAsync(user);
            if (res == null)
            {
                return Ok(new StatusNew(400, "Server Error"));
            }
            return Ok(new StatusNew(200, "Registration Successfull"));
           
        }

        [HttpPost]
        public async Task<IActionResult> forgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new StatusNew(400, "please pass all the valid fields"));
            }
            var checkpass = _authService.validiatePassword(model.NewPassword);
            if (checkpass != null)
            {
                return Ok(new StatusNew(400, checkpass));
            }
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                return Ok(new StatusNew(400, "New Password and Confirm password should be same."));
            }
            var user = await _authService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new StatusNew(400, "Invalid Email"));
            }
            VerifyModel otpInstance = new VerifyModel
            {
                Email=model.Email,
                Otp=model.Otp
            };
            var result = await _authService.CheckOtpAsync(otpInstance);
            if (result != null)
            {
                return Ok(new StatusNew(400, result));
            }
            var res = await _authService.ChangePasswordAsync(user, model.NewPassword);
            if (res == null)
            {
                return Ok(new StatusNew(400, "Change Password failed"));
            }
            return Ok(new StatusNew(200, "Password has changed successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new StatusNew(400, "please pass all the valid fields"));
            }
            var user = await _authService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new StatusNew(400, "User not Exist"));
            }
            if (!await _authService.CheckPasswordAsync(user, model.Password))
            {
                return Ok(new StatusNew(400, "incorrect password"));
            }
            if (!user.IsVerified)
            {
                var send = await _authService.VerifyEmailAsync(user.Email);
                return Ok(new StatusNew(202, "Otp Send to Your Email, Please verify your email"));
            }
            var result = await _authService.Login(model);
            if (result == null)
            {
                return Ok(new StatusNew(400, "Login failed"));
            }
            return Ok(new Status(200, "Login success", result)); ;
        }
    }
}
