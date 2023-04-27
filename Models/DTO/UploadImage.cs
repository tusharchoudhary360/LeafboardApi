using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models.DTO
{
    public class UploadImage
    {
        [Required]
        public IFormFile formFile { get; set; }
    }
}
