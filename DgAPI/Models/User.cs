using System;
using System.ComponentModel.DataAnnotations;

namespace DgAPI.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string user_fullname { get; set; }

        [Required]
        [MaxLength(100)]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string email { get; set; }

        [Required]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;

        public string role { get; set; }
    }
}
