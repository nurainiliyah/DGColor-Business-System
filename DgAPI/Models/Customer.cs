using System;
using System.ComponentModel.DataAnnotations;

namespace DgAPI.Models
{
    public class Customer
    {
        [Key]
        public int customer_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string customer_name { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string customer_contactNum { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string customer_email { get; set; }

        [Required]
        public DateTime custcreatedAt { get; set; } = DateTime.UtcNow;
    }
}

