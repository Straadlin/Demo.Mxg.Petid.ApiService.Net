using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class TypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Type>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Type> builder)
    {
        // Fluent API
        builder.ToTable("TYPE").HasKey(t => t.Id);

        //

        builder.HasOne(t => t.CollectionType)
               .WithMany(c => c.Types)
               .HasForeignKey(t => t.CollectionTypeId);

        //

        builder.HasMany(t => t.AlgorithmPasswordTypeUsers)
               .WithOne(u => u.AlgorithmPasswordType)
               .HasForeignKey(u => u.AlgorithmPasswordTypeId);

        builder.HasMany(t => t.GenderTypeUsers)
               .WithOne(u => u.GenderType)
               .HasForeignKey(u => u.GenderTypeId);

        builder.HasMany(t => t.StatusAccountTypeUsers)
               .WithOne(u => u.StatusAccountType)
               .HasForeignKey(u => u.StatusAccountTypeId);

        builder.HasMany(c => c.IdentifierTags)
               .WithOne(t => t.TagType)
               .HasForeignKey(t => t.TagTypeId);

        builder.HasMany(c => c.GenderTypePets)
               .WithOne(t => t.GenderType)
               .HasForeignKey(t => t.GenderTypeId);

        builder.HasMany(c => c.SpecieTypes)
               .WithOne(t => t.SpecieType)
               .HasForeignKey(t => t.SpecieTypeId);

        builder.HasMany(c => c.ExtensionFileTypeResources)
               .WithOne(t => t.ExtensionFileType)
               .HasForeignKey(t => t.ExtensionFileTypeId);

        //

        builder.Property(e => e.Id).HasColumnName("TYPE_ID").IsRequired();
        builder.Property(e => e.Code).HasColumnName("CODE").IsRequired();
        builder.Property(e => e.Description).HasColumnName("DESCRIPTION").IsRequired(false);
        builder.Property(e => e.CollectionTypeId).HasColumnName("COLLECTION_TYPE_ID").IsRequired();

        builder.Property(e => e.InUse).HasColumnName("IN_USE").IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.LastModifiedDate).HasColumnName("LAST_MODIFIED_DATE").IsRequired(false);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired(false);
        builder.Property(e => e.ModifiedCount).HasColumnName("MODIFIED_COUNT").IsRequired();
    }
}