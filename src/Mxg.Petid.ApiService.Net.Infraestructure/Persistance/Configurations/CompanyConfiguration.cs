using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        // Fluent API
        builder.ToTable("COMPANY").HasKey(e => e.Id);

        builder.HasMany(c => c.Users)
               .WithOne(u => u.EmployeedCompany)
               .HasForeignKey(u => u.EmployeedCompanyId);

        builder.HasOne(c => c.OwnerUser)
               .WithMany(u => u.Companies)
               .HasForeignKey(c => c.OwnerUserId);

        builder.HasOne(c => c.City)
               .WithMany(c => c.Companies)
               .HasForeignKey(c => c.CityId);

        builder.Property(e => e.Id).HasColumnName("COMPANY_ID").IsRequired();
        builder.Property(e => e.IdentificationCode).HasColumnName("IDENTIFICATION_CODE").IsRequired();
        builder.Property(e => e.Name).HasColumnName("NAME").IsRequired();
        builder.Property(e => e.Name).HasColumnName("ADDRESS").IsRequired(false);
        builder.Property(e => e.OwnerUserId).HasColumnName("OWNER_USER_ID").IsRequired(false);
        builder.Property(e => e.CityId).HasColumnName("CITY_ID").IsRequired(false);

        builder.Property(e => e.InUse).HasColumnName("IN_USE").IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.LastModifiedDate).HasColumnName("LAST_MODIFIED_DATE").IsRequired(false);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired(false);
        builder.Property(e => e.ModifiedCount).HasColumnName("MODIFIED_COUNT").IsRequired();
    }
}