using System.ComponentModel.DataAnnotations;

namespace WorkShop.Domain
{
    public class InvoiceDetailView
    {
        public string Id { get; set; }
        public string InvoiceId { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public ProductView ProductView { get; set; }

        [Required]
        public double Price { get; set; }
        public double Total { get; set; }
    }
}
