using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        // Fluent API
        builder.ToTable("ROLE_PERMISSION").HasKey(e => new { e.RoleId, e.PermissionId });

        builder.HasOne(r => r.Role)
               .WithMany(r => r.RolePermissions)
               .HasForeignKey(r => r.RoleId);

        builder.HasOne(r => r.Permission)
               .WithMany(p => p.RolePermissions)
               .HasForeignKey(r => r.PermissionId);

        builder.Property(e => e.RoleId).HasColumnName("ROLE_ID").IsRequired();
        builder.Property(e => e.PermissionId).HasColumnName("PERMISSION_ID").IsRequired();

        builder.Property(e => e.InUse).HasColumnName("IN_USE").IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.LastModifiedDate).HasColumnName("LAST_MODIFIED_DATE").IsRequired(false);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired(false);
        builder.Property(e => e.ModifiedCount).HasColumnName("MODIFIED_COUNT").IsRequired();

        builder.Ignore(e => e.Id);
    }
}