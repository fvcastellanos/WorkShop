using System;

namespace WorkShop.Domain
{
    public class ProductView
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinimalAmount { get; set; }
        public double SalePrice { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int Active { get; set; }        
    }
}