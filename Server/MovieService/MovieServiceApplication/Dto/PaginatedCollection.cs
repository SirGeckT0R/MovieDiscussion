namespace MovieServiceApplication.Dto
{
    public class PaginatedCollection<T>
    {
        public ICollection<T> Items { get; set; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedCollection(ICollection<T> items, int pageIndex, int totalPages)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }

        public PaginatedCollection()
        {
        }
    }
}
