using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        // Fluent API
        builder.ToTable("PET").HasKey(e => e.Id);

        //

        builder.HasOne(p => p.GenderType)
               .WithMany(t => t.GenderTypePets)
               .HasForeignKey(u => u.GenderTypeId);

        builder.HasOne(p => p.SpecieType)
               .WithMany(t => t.SpecieTypes)
               .HasForeignKey(u => u.SpecieTypeId);

        builder.HasOne(p => p.IdentifierTag)
               .WithOne(cu => cu.Pet)
               .HasForeignKey<Pet>(c => c.IdentifierTagId);// Es así porque es una relación 1 a 1

        builder.HasOne(p => p.City)
               .WithMany(p => p.Pets)
               .HasForeignKey(v => v.CityId);

        //

        builder.HasMany(c => c.Vaccines)
               .WithOne(t => t.Pet)
               .HasForeignKey(t => t.PetId);

        builder.HasMany(c => c.Resources)
               .WithOne(t => t.Pet)
               .HasForeignKey(t => t.PetId);

        //

        builder.Property(e => e.Id).HasColumnName("PET_ID").IsRequired();
        builder.Property(e => e.Name).HasColumnName("NAME").IsRequired(false);
        builder.Property(e => e.Birthdate).HasColumnName("BIRTHDATE").IsRequired(false);
        builder.Property(e => e.Breed).HasColumnName("BREED").IsRequired(false);
        builder.Property(e => e.Color).HasColumnName("COLOR").IsRequired(false);
        builder.Property(e => e.Weight).HasColumnName("WEIGHT").IsRequired(false);
        builder.Property(e => e.DistinctiveFeature).HasColumnName("DISTINCTIVE_FEATURE").IsRequired(false);
        builder.Property(e => e.Notes).HasColumnName("NOTES").IsRequired(false);
        builder.Property(e => e.PublicOwner).HasColumnName("PUBLIC_OWNER").IsRequired(false);
        builder.Property(e => e.PublicPhoneNumber).HasColumnName("PUBLIC_PHONE_NUMBER").IsRequired(false);
        builder.Property(e => e.PublicEmail).HasColumnName("PUBLIC_EMAIL").IsRequired(false);
        builder.Property(e => e.PublicAddress).HasColumnName("PUBLIC_ADDRESS").IsRequired(false);
        builder.Property(e => e.GenderTypeId).HasColumnName("GENDER_TYPE_ID").IsRequired(false);
        builder.Property(e => e.SpecieTypeId).HasColumnName("SPECIE_TYPE_ID").IsRequired(false);
        builder.Property(e => e.IdentifierTagId).HasColumnName("IDENTIFIER_TAG_ID").IsRequired();
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