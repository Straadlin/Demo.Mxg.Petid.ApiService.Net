using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        // Fluent API
        builder.ToTable("SESSION").HasKey(e => e.Id);

        builder.HasOne(s => s.User)
               .WithMany(u => u.Sessions)
               .HasForeignKey(s => s.UserId);

        builder.Property(e => e.Id).HasColumnName("SESSION_ID").IsRequired();
        builder.Property(e => e.RefreshToken).HasColumnName("REFRESH_TOKEN").IsRequired(false);
        builder.Property(e => e.RefreshTokenExpire).HasColumnName("REFRESH_TOKEN_EXPIRE").IsRequired();
        builder.Property(e => e.RefreshTokenIsUsed).HasColumnName("REFRESH_TOKEN_IS_USED").IsRequired();
        builder.Property(e => e.RefreshTokenIsRevoked).HasColumnName("REFRESH_TOKEN_IS_REVOKED").IsRequired();
        builder.Property(e => e.IpAddress).HasColumnName("IP_ADDRESS").IsRequired(false);
        builder.Property(e => e.Latitude).HasColumnName("LATITUDE").IsRequired(false);
        builder.Property(e => e.Longitude).HasColumnName("LONGITUDE").IsRequired(false);
        builder.Property(e => e.Altitude).HasColumnName("ALTITUDE").IsRequired(false);
        builder.Property(e => e.UserId).HasColumnName("USER_ID").IsRequired();

        builder.Property(e => e.InUse).HasColumnName("IN_USE").IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.LastModifiedDate).HasColumnName("LAST_MODIFIED_DATE").IsRequired(false);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired(false);
        builder.Property(e => e.ModifiedCount).HasColumnName("MODIFIED_COUNT").IsRequired();
    }
}