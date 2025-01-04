using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Fluent API
        builder.ToTable("USER").HasKey(u => u.Id);

        // ---

        builder.HasOne(u => u.GenderType)
               .WithMany(t => t.GenderTypeUsers)
               .HasForeignKey(u => u.GenderTypeId);

        builder.HasOne(u => u.AlgorithmPasswordType)
               .WithMany(t => t.AlgorithmPasswordTypeUsers)
               .HasForeignKey(u => u.AlgorithmPasswordTypeId);

        builder.HasOne(u => u.City)
               .WithMany(c => c.Users)
               .HasForeignKey(u => u.CityId);

        builder.HasOne(u => u.EmployeedCompany)
               .WithMany(c => c.Users)
               .HasForeignKey(u => u.EmployeedCompanyId);

        builder.HasOne(u => u.StatusAccountType)
               .WithMany(t => t.StatusAccountTypeUsers)
               .HasForeignKey(u => u.StatusAccountTypeId);

        builder.HasOne(u => u.Role)
               .WithMany(r => r.Users)
               .HasForeignKey(u => u.RoleId);

        // ---

        builder.HasMany(c => c.Sessions)
               .WithOne(s => s.User)
               .HasForeignKey(s => s.UserId);

        builder.HasMany(u => u.Companies)
               .WithOne(c => c.OwnerUser)
               .HasForeignKey(u => u.OwnerUserId);

        // ---

        builder.Property(e => e.Id).HasColumnName("USER_ID").IsRequired();
        builder.Property(e => e.Username).HasColumnName("USERNAME").IsRequired();
        builder.Property(e => e.Email).HasColumnName("EMAIL").IsRequired(false);
        builder.Property(e => e.EmailConfirmed).HasColumnName("EMAIL_CONFIRMED").IsRequired(true);
        builder.Property(e => e.EmailConfirmedCode).HasColumnName("EMAIL_CONFIRMED_CODE").IsRequired(false);
        builder.Property(e => e.PhoneNumber).HasColumnName("PHONE_NUMBER").IsRequired(false);
        builder.Property(e => e.PhoneNumberConfirmed).HasColumnName("PHONE_NUMBER_CONFIRMED").IsRequired();
        builder.Property(e => e.PhoneNumberConfirmedCode).HasColumnName("PHONE_NUMBER_CONFIRMED_CODE").IsRequired(false);
        builder.Property(e => e.Birthdate).HasColumnName("BIRTHDATE").IsRequired(false);
        builder.Property(e => e.PasswordHash).HasColumnName("PASSWORD_HASH").IsRequired(false);
        builder.Property(e => e.PrivateInfoJson).HasColumnName("PRIVATE_INFO_JSON").IsRequired(false);
        builder.Property(e => e.IsInfoEncrypted).HasColumnName("IS_INFO_ENCRYPTED").IsRequired();
        builder.Property(e => e.GenderTypeId).HasColumnName("GENDER_TYPE_ID").IsRequired(false);
        builder.Property(e => e.AlgorithmPasswordTypeId).HasColumnName("ALGORITHM_PASSWORD_TYPE_ID").IsRequired();
        builder.Property(e => e.CityId).HasColumnName("CITY_ID").IsRequired(false);
        builder.Property(e => e.EmployeedCompanyId).HasColumnName("EMPLOYEED_COMPANY_ID").IsRequired(false);
        builder.Property(e => e.StatusAccountTypeId).HasColumnName("STATUS_ACCOUNT_TYPE_ID").IsRequired();
        builder.Property(e => e.RoleId).HasColumnName("ROLE_ID").IsRequired();

        builder.Property(e => e.InUse).HasColumnName("IN_USE").IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.LastModifiedDate).HasColumnName("LAST_MODIFIED_DATE").IsRequired(false);
        builder.Property(e => e.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").IsRequired(false);
        builder.Property(e => e.ModifiedCount).HasColumnName("MODIFIED_COUNT").IsRequired();
    }
}