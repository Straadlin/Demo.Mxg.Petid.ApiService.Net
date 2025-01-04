using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        // Fluent API
        builder.ToTable("CITY").HasKey(c => c.Id);

        builder.HasOne(c => c.State)
               .WithMany(s => s.Cities)
               .HasForeignKey(c => c.StateId);

        //

        builder.HasMany(c => c.Users)
               .WithOne(u => u.City)
               .HasForeignKey(u => u.CityId);

        builder.HasMany(c => c.Companies)
               .WithOne(c => c.City)
               .HasForeignKey(c => c.CityId);

        builder.Property(e => e.Id).HasColumnName("CITY_ID").IsRequired();
        builder.Property(e => e.Code).HasColumnName("CODE").IsRequired();
        builder.Property(e => e.Name).HasColumnName("NAME").IsRequired(false);
        builder.Property(e => e.StateId).HasColumnName("STATE_ID").IsRequired();

        builder.Property(e => e.InUse).HasColumnName("IN_USE").IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.LastModifiedDate).HasColumnName("LAST_MODIFIED_DATE").IsRequired(false);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired(false);
        builder.Property(e => e.ModifiedCount).HasColumnName("MODIFIED_COUNT").IsRequired();
    }
}