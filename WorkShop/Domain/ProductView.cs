using System;
using System.ComponentModel.DataAnnotations;

namespace WorkShop.Domain
{
    public class ProductView
    {
        public string Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(1, 5000)]
        public int MinimalAmount { get; set; }
        public double SalePrice { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int Active { get; set; }        
    }
}