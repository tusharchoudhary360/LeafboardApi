using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models.Domain
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsVerified { get; set; }  
        [Required]
        public string PhoneNumber { get; set; }
        [Required] 
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public Hobby Hobby { get; set; }
        public string? ProfileImage { get; set; }
    }
    public enum Hobby
    {
        Sports,
        Reading,
        Movies,
        coding,
        singing,
    }
}
