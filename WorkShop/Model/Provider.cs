using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkShop.Model
{
    [Table("provider")]
    public class Provider
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

        [Column("contact", TypeName = "varchar(150)")]
        public string Contact { get; set; }

        [Column("tax_id", TypeName = "varchar(50)")]
        public string TaxId { get; set; }

        [Column("description", TypeName = "varchar(300)")]
        public string Description { get; set; }

        [Column("created", TypeName = "timestamp")]
        public DateTime Created { get; set; }

        [Column("updated", TypeName = "timestamp")]
        public DateTime Updated { get; set; }        
    }
}