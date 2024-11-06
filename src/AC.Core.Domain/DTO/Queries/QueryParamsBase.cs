namespace AC.Core.Domain.DTO.Queries
{
    public abstract class QueryParamsBase
    {
        public int CurrentPage { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 100;
        public bool Paginate { get; set; } = true;
        public int IgnoredItems
        {
            get
            {
                return (CurrentPage - 1) * ItemsPerPage;
            }
        }
    }
}