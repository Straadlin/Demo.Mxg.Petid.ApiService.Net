using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        // Fluent API
        builder.ToTable("STATE").HasKey(s => s.Id);

        builder.HasMany(s => s.Cities)
               .WithOne(c => c.State)
               .HasForeignKey(c => c.StateId);

        builder.HasOne(s => s.Country)
               .WithMany(c => c.States)
               .HasForeignKey(s => s.CountryId);

        builder.Property(e => e.Id).HasColumnName("TYPE_ID").IsRequired();
        builder.Property(e => e.Code).HasColumnName("CODE").IsRequired();
        builder.Property(e => e.Name).HasColumnName("NAME").IsRequired(false);
        builder.Property(e => e.CountryId).HasColumnName("COUNTRY_ID").IsRequired();

        builder.Property(e => e.InUse).HasColumnName("IN_USE").IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.LastModifiedDate).HasColumnName("LAST_MODIFIED_DATE").IsRequired(false);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired(false);
        builder.Property(e => e.ModifiedCount).HasColumnName("MODIFIED_COUNT").IsRequired();
    }
}