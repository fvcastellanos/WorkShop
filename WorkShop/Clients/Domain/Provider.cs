namespace WorkShop.Clients.Domain
{
    public class Provider
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double MinimalAmount { get; set; }
        public double SalePrice { get; set; }        
    }
}
