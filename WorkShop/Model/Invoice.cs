using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkShop.Model
{
    [Table("invoice")]
    public class Invoice
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
        
        [Required]
        [Column("kind", TypeName = "varchar(50)")]
        public string Kind { get; set; }

        [Required]
        [Column("type", TypeName = "varchar(50)")]
        public string Type { get; set; }

        [Column("image_url", TypeName = "varchar(250)")]
        public string ImageUrl { get; set; }

        [Column("due_date", TypeName = "timestamp")]
        public DateTime DueDate { get; set; }

        [Column("description", TypeName = "varchar(300)")]
        public string Description { get; set; }

        [Required]
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