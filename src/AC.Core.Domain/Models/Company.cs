namespace AC.Core.Domain.Models
{
    public class Company : BaseModel
    {
        public string? FantasyName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public string? Document { get; set; }
        public string? Address { get; set; }
        public bool? IsGroup { get; set; }
        public string? Number { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneExtension { get; set; }
        public string? District { get; set; }
        public string? Department { get; set; }
        public string? Description { get; set; }
        public bool? IsNational { get; set; }
        public virtual ICollection<User>? Users { get; set; }
    }
}