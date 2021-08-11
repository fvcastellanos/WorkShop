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
    }
}