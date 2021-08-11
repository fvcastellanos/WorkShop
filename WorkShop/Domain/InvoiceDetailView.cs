using System.ComponentModel.DataAnnotations;

namespace WorkShop.Domain
{
    public class InvoiceDetailView
    {
        public string Id { get; set; }
        public string InvoiceId { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public ProductView ProductView { get; set; }

        public double DiscountAmount { get; set; }

        [Required]
        public double Price { get; set; }
        public double Total { get; set; }
    }
}
