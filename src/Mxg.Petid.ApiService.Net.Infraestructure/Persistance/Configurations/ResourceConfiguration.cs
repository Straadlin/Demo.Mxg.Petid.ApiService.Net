using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        // Fluent API
        builder.ToTable("RESOURCE").HasKey(e => e.Id);

        //

        builder.HasOne(p => p.ExtensionFileType)
               .WithMany(p => p.ExtensionFileTypeResources)
               .HasForeignKey(v => v.ExtensionFileTypeId);

        builder.HasOne(p => p.StorageType)
               .WithMany(p => p.StorageTypeResources)
               .HasForeignKey(v => v.StorageTypeId);

        builder.HasOne(p => p.Pet)
               .WithMany(p => p.Resources)
               .HasForeignKey(v => v.PetId);

        builder.HasOne(p => p.Vaccine)
               .WithMany(p => p.Resources)
               .HasForeignKey(v => v.VaccineId);

        builder.HasOne(p => p.Product)
               .WithMany(p => p.Resources)
               .HasForeignKey(v => v.ProductId);

        //

        builder.Property(e => e.Id).HasColumnName("RESOURCE_ID").IsRequired();
        builder.Property(e => e.Uri).HasColumnName("URI").IsRequired();
        builder.Property(e => e.FileName).HasColumnName("FILE_NAME").IsRequired();
        builder.Property(e => e.ExtensionFileTypeId).HasColumnName("EXTENSION_FILE_TYPE_ID").IsRequired();
        builder.Property(e => e.StorageTypeId).HasColumnName("STORAGE_TYPE_ID").IsRequired();
        builder.Property(e => e.PetId).HasColumnName("PET_ID").IsRequired(false);
        builder.Property(e => e.VaccineId).HasColumnName("VACCINE_ID").IsRequired(false);
        builder.Property(e => e.ProductId).HasColumnName("PRODUCT_ID").IsRequired(false);

        builder.Property(e => e.InUse).HasColumnName("IN_USE").IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.LastModifiedDate).HasColumnName("LAST_MODIFIED_DATE").IsRequired(false);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired(false);
        builder.Property(e => e.ModifiedCount).HasColumnName("MODIFIED_COUNT").IsRequired();
    }
}