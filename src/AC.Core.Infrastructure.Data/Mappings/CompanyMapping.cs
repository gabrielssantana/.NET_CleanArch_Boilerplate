using AC.Core.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AC.Core.Infrastructure.Data.Mappings
{
    public class CompanyMapping : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder
            .HasKey(c => c.Id)
            .HasName("PK_Companies");

            builder
            .Property(c => c.Name)
            .HasColumnName("Name")
            .HasMaxLength(200);

            builder
            .Property(c => c.Document)
            .HasColumnName("Document")
            .HasMaxLength(200);

            builder
            .Property(c => c.Address)
            .HasColumnName("Address")
            .HasMaxLength(200);

            builder
            .Property(c => c.IsGroup)
            .HasColumnName("IsGroup");

            builder
            .Property(c => c.FantasyName)
            .HasColumnName("FantasyName")
            .HasMaxLength(200);

            builder
            .Property(c => c.Email)
            .HasColumnName("Email")
            .HasMaxLength(200);

            builder
            .Property(c => c.Contact)
            .HasColumnName("Contact")
            .HasMaxLength(200);

            builder
            .Property(c => c.Number)
            .HasColumnName("Number")
            .HasMaxLength(200);

            builder
            .Property(c => c.ZipCode)
            .HasColumnName("ZipCode")
            .HasMaxLength(200);

            builder
            .Property(c => c.City)
            .HasColumnName("City")
            .HasMaxLength(200);

            builder
            .Property(c => c.State)
            .HasColumnName("State")
            .HasMaxLength(200);

            builder
            .Property(c => c.PhoneNumber)
            .HasColumnName("PhoneNumber")
            .HasMaxLength(200);

            builder
            .Property(c => c.PhoneExtension)
            .HasColumnName("PhoneExtension")
            .HasMaxLength(200);

            builder
            .Property(c => c.District)
            .HasColumnName("District")
            .HasMaxLength(200);

            builder
            .Property(c => c.Department)
            .HasColumnName("Department")
            .HasMaxLength(200);

            builder
            .Property(c => c.Description)
            .HasColumnName("Description")
            .HasMaxLength(200);

            builder
            .Property(c => c.IsNational)
            .HasColumnName("IsNational");

            builder
            .HasMany(e => e.Users)
            .WithOne(e => e.Company)
            .HasForeignKey(e => e.CompanyId)
            .HasPrincipalKey(e => e.Id)
            .HasConstraintName("FK_Users_Companies");
        }
    }
}