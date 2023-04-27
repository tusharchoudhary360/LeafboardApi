﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models.Domain
{
    public class UserImages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string? UserImage { get; set; }
    }
}
