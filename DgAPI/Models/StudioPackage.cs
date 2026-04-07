using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DgAPI.Models
{
    public class StudioPackage
    {
        [Key]
        public int package_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string package_name { get; set; }

        [Required]
        [MaxLength(200)]
        public string package_description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal package_price { get; set; }

        [MaxLength]
        public string package_note { get; set; }

        [Required]
        [MaxLength(20)]
        public string package_status { get; set; }
    }
}