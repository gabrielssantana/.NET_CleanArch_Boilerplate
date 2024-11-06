namespace AC.Core.Domain.Models
{
    public class User : BaseModel
    {
        public required string Name { get; set; }
        public required string Document { get; set; }
        public string? Phone { get; set; }
        public bool? Status { get; set; }
        public required string Email { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company? Company { get; set; }
    }
}