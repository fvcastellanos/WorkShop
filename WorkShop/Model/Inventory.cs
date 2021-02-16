using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkShop.Model
{
    [Table("inventory")]
    public class Inventory
    {
        [Key]
        [Column("id", TypeName = "varchar(50)")]
        public string Id { get; set; }

        [ForeignKey("product_id")]
        public Product Product { get; set; }

        [ForeignKey("operation_type_id")]
        public OperationType OperationType { get; set; }

        [ForeignKey("provider_invoice_id")]
        public ProviderInvoice ProviderInvoice { get; set; }

        [Required]
        [Column("amount")]
        public double Amount { get; set; }

        [Required]
        [Column("unit_price")]
        public double UnitPrice { get; set; }

        [ForeignKey("discount_type_id")]
        public DiscountType DiscountType { get; set; }

        [Column("discount_value")]
        public double DiscountValue { get; set; }

        [Required]
        [Column("total")]
        public double Total { get; set; }

        [Required]
        [Column("created", TypeName = "timestamp")]
        public DateTime Created { get; set; }

        [Column("updated", TypeName = "timestamp")]
        public DateTime Updated { get; set; }
    }
}