using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkShop.Model
{
    [Table("provider_invoice")]
    public class ProviderInvoice
    {
        [Key]
        [Column("id", TypeName = "varchar(50)")]
        public Guid Id { get; set; }

        [ForeignKey("provider_id")]
        public Provider Provider { get; set; }

        [Required]
        [Column("serial", TypeName = "varchar(50)")]
        public string Serial { get; set; }

        [Required]
        [Column("number", TypeName = "varchar(100)")]
        public string Number { get; set; }
        

        [Column("image_url", TypeName = "varchar(250)")]
        public string ImageUrl { get; set; }

        [Column("active")]
        public int Active { get; set; }

        [Column("created", TypeName = "timestamp")]
        public DateTime Created { get; set; }

        [Column("updated", TypeName = "timestamp")]
        public DateTime Updated { get; set; }        

        [Required]
        [Column("tenant", TypeName = "varchar(50)")]
        public string Tenant { get; set; }

    }
}