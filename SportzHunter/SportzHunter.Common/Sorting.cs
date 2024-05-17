namespace SportzHunter.Common
{
    public class Sorting
    {
        public string SortBy { get; set; }
        public string SortOrder { get; set; }

        public Sorting(string SortBy, string SortOrder)
        {
            this.SortBy = SortBy;
            this.SortOrder = SortOrder;
        }

    }
}
