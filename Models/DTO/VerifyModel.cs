using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models.DTO
{
    public class VerifyModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public int Otp { get; set; }
    }
}
