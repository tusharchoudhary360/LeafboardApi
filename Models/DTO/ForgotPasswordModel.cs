using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models.DTO
{
    public class ForgotPasswordModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public int Otp { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}
