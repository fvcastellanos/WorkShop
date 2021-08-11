using System;
using System.ComponentModel.DataAnnotations;

namespace WorkShop.Domain
{
    public class InvoiceView
    {
        public string Id { get; set; }

        [Required]
        public ProviderView ProviderView { get; set; }

        [Required]
        [MaxLength(50)]
        public string Serial { get; set; }

        [Required]
        [MaxLength(150)]
        public string Number { get; set; }

        [Required]
        public double Amount { get; set; }

        // [Required]
        [MaxLength(10)]
        public string Type { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public int Active { get; set; }
        public string ImageUrl { get; set; }

        public double Total { get; set; }
    }
}