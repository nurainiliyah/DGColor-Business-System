using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DgAPI.Models
{
    public class StudioBooking
    {
        [Key]
        public int booking_id { get; set; }

        [Required]
        public int customer_id { get; set; }  // Foreign Key referencing Customer

        [ForeignKey("customer_id")]
        public Customer? customers { get; set; }

        [Required]
        public int package_id { get; set; }  // Foreign Key referencing Package

        [ForeignKey("package_id")]
        public StudioPackage? studioPackages { get; set; }

        [Required]
        public DateOnly booking_date { get; set; }

        [Required]
        public TimeOnly booking_time { get; set; }

        [MaxLength]
        public string note { get; set; }

        [Required]
        public DateTime bookingcreatedAt { get; set; } = DateTime.UtcNow;
    }
}
