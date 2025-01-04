using Microsoft.EntityFrameworkCore;
using Mxg.Petid.ApiService.Net.Domain.Common;
using Mxg.Petid.ApiService.Net.Domain.Entities;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Configurations;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance;

public class PetidDbContext : DbContext
{
    public PetidDbContext(DbContextOptions<PetidDbContext> options) : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.InUse = false;
                    entry.Entity.IsActive = true;
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.CreatedBy = string.IsNullOrEmpty(entry.Entity.CreatedBy) ? "0" : entry.Entity.CreatedBy;
                    entry.Entity.ModifiedCount = 0;
                    break;
                case EntityState.Modified:
                    entry.Entity.InUse = false;
                    entry.Entity.LastModifiedDate = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = string.IsNullOrEmpty(entry.Entity.LastModifiedBy) ? "0" : entry.Entity.LastModifiedBy;
                    entry.Entity.ModifiedCount = entry.Entity.ModifiedCount + 1;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CityConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CollectionTypeConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CompanyConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CountryConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentifierTagConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PermissionConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResourceConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoleConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RolePermissionConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SessionConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StateConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TypeConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VaccineConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<CollectionType> CollectionTypes { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<IdentifierTag> IdentifierTags { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<Domain.Entities.Type> Types { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Vaccine> Vaccines { get; set; }
}