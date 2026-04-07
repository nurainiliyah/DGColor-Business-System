using System;
using System.ComponentModel.DataAnnotations;
namespace DgAPI.Models
{
    public class Report
    {
        [Key]
        public int report_id { get; set; }

        [Required]
        [MaxLength(50)]
        public string report_type { get; set; }

        [Required]
        public DateOnly generated_date { get; set; }
    }
}
