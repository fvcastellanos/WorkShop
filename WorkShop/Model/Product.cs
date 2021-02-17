using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkShop.Model
{
    [Table("product")]
    public class Product
    {
        [Key]
        [Column("id", TypeName = "varchar(50)")]
        public string Id { get; set; }

        [Required]
        [Column("code", TypeName = "varchar(50)")]
        public string Code { get; set; }

        [Required]
        [Column("name", TypeName = "varchar(150)")]
        public string Name { get; set; }

        [Column("description", TypeName = "varchar(300)")]
        public string Description { get; set; }

        [Required]
        [Column("minimal_amount")]
        public int MinimalAmount { get; set; }

        [Required]
        [Column("sale_price")]
        public double SalePrice { get; set; }

        [Required]
        [Column("created", TypeName = "timestamp")]
        public DateTime Created { get; set; }

        [Column("updated", TypeName = "timestamp")]
        public DateTime Updated { get; set; }

        [Required]
        [Column("active")]
        public int Active { get; set; }
    }
}