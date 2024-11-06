using AC.Core.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AC.Core.Infrastructure.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
            .ToTable("Users");

            builder
            .HasKey(u => u.Id)
            .HasName("PK_Users");

            builder
            .Property(u => u.Name)
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

            builder
            .Property(u => u.Document)
            .HasColumnName("Document")
            .HasMaxLength(14)
            .IsRequired();

            builder
            .Property(u => u.Phone)
            .HasColumnName("Phone")
            .HasMaxLength(200);

            builder
            .Property(u => u.Status)
            .HasColumnName("Status");

            builder
            .Property(u => u.Email)
            .HasColumnName("Email")
            .HasMaxLength(100)
            .IsRequired();

            builder
            .HasIndex(b => b.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

            builder
            .HasIndex(b => b.CompanyId)
            .HasDatabaseName("IX_Users_CompanyId");
        }
    }
}