namespace WorkShop.Domain
{
    public class InvoiceSearchView : SearchView
    {
        public string Serial { get; set; }
        public string Number { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderName { get; set; }        
    }
}