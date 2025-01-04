using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class IdentifierTagConfiguration : IEntityTypeConfiguration<IdentifierTag>
{
    public void Configure(EntityTypeBuilder<IdentifierTag> builder)
    {
        // Fluent API
        builder.ToTable("IDENTIFIER_TAG").HasKey(e => e.Id);

        builder.HasOne(it => it.TagType)
               .WithMany(t => t.IdentifierTags)
               .HasForeignKey(t => t.TagTypeId);

        //

        builder.HasOne(it => it.Pet)
               .WithOne(c => c.IdentifierTag)
               .HasForeignKey<Pet>(c => c.IdentifierTagId);// Es así porque es una relación 1 a 1

        builder.Property(e => e.Id).HasColumnName("IDENTIFIER_TAG_ID").IsRequired();
        builder.Property(e => e.PublicIdentifierTag).HasColumnName("PUBLIC_INDENTIFIER_TAG").IsRequired();
        builder.Property(e => e.SalePrice).HasColumnName("SALE_PRICE").IsRequired(false);
        builder.Property(e => e.PurchaseCost).HasColumnName("PURCHASE_COST").IsRequired(false);
        builder.Property(e => e.TagTypeId).HasColumnName("TAG_TYPE_ID").IsRequired();

        builder.Property(e => e.InUse).HasColumnName("IN_USE").IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.LastModifiedDate).HasColumnName("LAST_MODIFIED_DATE").IsRequired(false);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired(false);
        builder.Property(e => e.ModifiedCount).HasColumnName("MODIFIED_COUNT").IsRequired();
    }
}