using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkShop.Model
{
    [Table("operation_type")]
    public class OperationType
    {
        [Key]
        [Column("id", TypeName = "varchar(50)")]
        public Guid Id { get; set; }

        [Required]
        [Column("name", TypeName = "varchar(150)")]
        public string Name { get; set; }

        [Column("description", TypeName = "varchar(300)")]
        public string Description { get; set; }

        [Column("inbound")]
        public int Inbound { get; set; }

        [Required]
        [Column("created", TypeName = "timestamp")]
        public DateTime Created { get; set; }

        [Column("updated", TypeName = "timestamp")]
        public DateTime Updated { get; set; }

        [Required]
        [Column("active")]
        public int Active { get; set; }

        [Required]
        [Column("tenant", TypeName = "varchar(50)")]
        public string Tenant { get; set; }
    }
}