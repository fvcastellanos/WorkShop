using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkShop.Model
{
    [Table("invoice_detail")]
    public class InvoiceDetail
    {
        [Key]
        [Column("id", TypeName = "varchar(50)")]
        public Guid Id { get; set; }

        [ForeignKey("invoice_id")]
        public Invoice Invoice { get; set; }

        [ForeignKey("product_id")]
        public Product Product { get; set; }

        [Required]
        [Column("quantity")]
        public double Quantity { get; set; }

        [Required]
        [Column("price")]
        public double Price { get; set; }

        [Column("discount_amount")]
        public double DiscountAmount { get; set; }

        [Required]
        [Column("total")]
        public Double Total { get; set; }

        [Required]
        [Column("created", TypeName = "timestamp")]
        public DateTime Created { get; set; }
    }
}