using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Fluent API
        builder.ToTable("PRODUCT").HasKey(e => e.Id);

        builder.HasMany(p => p.Resources)
               .WithOne(u => u.Product)
               .HasForeignKey(u => u.ProductId);

        builder.Property(e => e.Id).HasColumnName("PRODUCT_ID").IsRequired();
        builder.Property(e => e.Code).HasColumnName("CODE").IsRequired();
        builder.Property(e => e.Name).HasColumnName("NAME").IsRequired();
        builder.Property(e => e.Description).HasColumnName("DESCRIPTION").IsRequired();
        builder.Property(e => e.SalePrice).HasColumnName("SALE_PRICE").IsRequired();
        builder.Property(e => e.PurchasePrice).HasColumnName("PURCHASE_PRICE").IsRequired();

        builder.Property(e => e.InUse).HasColumnName("IN_USE").IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.LastModifiedDate).HasColumnName("LAST_MODIFIED_DATE").IsRequired(false);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired(false);
        builder.Property(e => e.ModifiedCount).HasColumnName("MODIFIED_COUNT").IsRequired();
    }
}