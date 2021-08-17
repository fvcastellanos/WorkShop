using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkShop.Model
{
    [Table("order")]
    public class Order
    {
        [Key]
        [Column("id", TypeName = "varchar(50)")]
        public Guid Id { get; set; }

        [ForeignKey("customer_id")]
        public Customer Customer { get; set; }

        [Column("plate_number")]
        public string PlateNumber { get; set; }

        [Column("created", TypeName = "timestamp")]
        public DateTime Created { get; set; }

        [Column("image_url", TypeName = "varchar(500)")]
        public string ImageUrl { get; set; }
    }
}