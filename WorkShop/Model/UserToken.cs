using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkShop.Model
{
    [Table("user_token")]
    public class UserToken
    {
        [Key]
        [Column("id", TypeName = "varchar(50)")]
        public Guid Id { get; set; }

        [Required]
        [Column("user", TypeName = "varchar(50)")]
        public string User { get; set; }

        [Required]
        [Column("token", TypeName = "varchar(150)")]
        public string Token { get; set; }
    }
}