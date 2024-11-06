using AC.Core.Domain.Interfaces.Repositories;

namespace AC.Core.Domain.Models
{
    public abstract class BaseModel : IAggregateRoot
    {
        public int Id { get; set; }
    }
}