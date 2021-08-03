using System;
using System.ComponentModel.DataAnnotations;

namespace WorkShop.Domain
{
    public class ProviderInvoiceView
    {
        public string Id { get; set; }
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderTaxId { get; set; }

        [MaxLength(50)]
        public string Serial { get; set; }

        [Required]
        [MaxLength(150)]
        public string Number { get; set; }

        [Required]
        public double Amount { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public int Active { get; set; }

        // public string ImageUrl { get; set; }
    }
}