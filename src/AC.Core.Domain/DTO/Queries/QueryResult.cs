namespace AC.Core.Domain.DTO.Queries
{
    public class QueryResult<T>
    where T : class
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages
        {
            get
            {
                if (ItemsPerPage == 0)
                {
                    return 0;
                }

                return (int)Math.Ceiling((double)TotalItems / (double)ItemsPerPage);
            }
        }
        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }
        public bool HasNextPage
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    }
}