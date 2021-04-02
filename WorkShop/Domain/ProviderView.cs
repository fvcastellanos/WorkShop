using System.ComponentModel.DataAnnotations;

namespace WorkShop.Domain
{
    public class ProviderView
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string  Contact { get; set; }

        [MaxLength(50)]
        public string TaxId { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }
        public int Active { get; set; }
    }
}