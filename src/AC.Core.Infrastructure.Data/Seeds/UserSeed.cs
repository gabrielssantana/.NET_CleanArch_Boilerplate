using AC.Core.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AC.Core.Infrastructure.Data.Seeds
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User
            {
                Id = -1,
                Name = "Administrador",
                Document = "00000000000",
                Email = "administrador@mail.com"
            });
        }
    }
}