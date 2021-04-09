using System.ComponentModel.DataAnnotations;

namespace WorkShop.Domain
{
    public class DiscountTypeView
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }
        public int Active { get; set; }
    }
}