namespace WorkShop.Domain
{
    public class SearchView
    {
        public string Code { get; set; }
        public int TopRows { get; set; }
        public string Name { get; set; }
        public int Active { get; set; }

        public string Serial { get; set; }
        public string Number { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}