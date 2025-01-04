using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class VaccineConfiguration : IEntityTypeConfiguration<Vaccine>
{
    public void Configure(EntityTypeBuilder<Vaccine> builder)
    {
        // Fluent API
        builder.ToTable("VACCINE").HasKey(e => e.Id);

        //

        builder.HasOne(p => p.Pet)
               .WithMany(p => p.Vaccines)
               .HasForeignKey(v => v.PetId);

        //

        builder.HasMany(c => c.Resources)
               .WithOne(t => t.Vaccine)
               .HasForeignKey(t => t.VaccineId);

        //

        builder.Property(e => e.Id).HasColumnName("VACCINE_ID").IsRequired();
        builder.Property(e => e.Name).HasColumnName("NAME").IsRequired();
        builder.Property(e => e.Detail).HasColumnName("DETAIL").IsRequired(false);
        builder.Property(e => e.VaccineApplied).HasColumnName("VACCINE_APPLIED").IsRequired();
        builder.Property(e => e.NextVaccine).HasColumnName("NEXT_VACCINE").IsRequired(false);
        builder.Property(e => e.PetId).HasColumnName("PET_ID").IsRequired();
        builder.Property(e => e.AppliedByCompanyId).HasColumnName("APPLIED_BY_COMPANY_ID").IsRequired(false);

        builder.Property(e => e.InUse).HasColumnName("IN_USE").IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.LastModifiedDate).HasColumnName("LAST_MODIFIED_DATE").IsRequired(false);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired(false);
        builder.Property(e => e.ModifiedCount).HasColumnName("MODIFIED_COUNT").IsRequired();
    }
}