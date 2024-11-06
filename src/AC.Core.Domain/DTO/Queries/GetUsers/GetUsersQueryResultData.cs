namespace AC.Core.Domain.DTO.Queries.GetUsers
{
    public class GetUsersQueryResultData
    {
        public string? Name { get; set; }
        public string? Document { get; set; }
        public string? Phone { get; set; }
        public bool? Status { get; set; }
        public string? Email { get; set; }
        public int? CompanyId { get; set; }
    }
}