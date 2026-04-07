using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DgAPI.Models
{
    public class PrintingService
    {
        [Key]
        public int printingservice_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string printingservice_name { get; set; }

        [Required]
        [MaxLength(50)]
        public string printing_size { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal printing_price { get; set; }

        [Required]
        [MaxLength(20)]
        public string printing_status { get; set; }
    }
}
