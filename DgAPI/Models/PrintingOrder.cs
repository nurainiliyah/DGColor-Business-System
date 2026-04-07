using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DgAPI.Models
{
    public class PrintingOrder
    {
        [Key]
        public int printingorder_id { get; set; }

        [Required]
        public int customer_id { get; set; }  // Foreign Key referencing Customer

        [ForeignKey("customer_id")]
        public Customer? customers { get; set; }

        [Required]
        public int printingservice_id { get; set; }  // Foreign Key referencing Print Service

        [ForeignKey("printingservice_id")]
        public PrintingService? printingServices { get; set; }

        [Required]
        public int printing_quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal print_totalprice { get; set; }

        [Required]
        public DateTime print_orderdate { get; set; }
    }
}
