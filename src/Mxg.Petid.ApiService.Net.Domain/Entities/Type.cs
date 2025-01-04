using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Type entity.
/// </summary>
public class Type : BaseAuditableEntity
{
    public Type()
    {
        Code = string.Empty;
        Description = string.Empty;
        AlgorithmPasswordTypeUsers = new HashSet<User>();
        GenderTypeUsers = new HashSet<User>();
        StatusAccountTypeUsers = new HashSet<User>();
        GenderTypePets = new HashSet<Pet>();
        SpecieTypes = new HashSet<Pet>();
        IdentifierTags = new HashSet<IdentifierTag>();
        ExtensionFileTypeResources = new HashSet<Resource>();
        StorageTypeResources = new HashSet<Resource>();
    }

    public string Code { get; set; }
    public string Description { get; set; }
    public Guid CollectionTypeId { get; set; }

    public virtual CollectionType? CollectionType { get; set; }
    public virtual ICollection<User> AlgorithmPasswordTypeUsers { get; set; }
    public virtual ICollection<User> GenderTypeUsers { get; set; }
    public virtual ICollection<User> StatusAccountTypeUsers { get; set; }
    public virtual ICollection<Pet> GenderTypePets { get; set; }
    public virtual ICollection<Pet> SpecieTypes { get; set; }
    public virtual ICollection<IdentifierTag> IdentifierTags { get; set; }
    public virtual ICollection<Resource> ExtensionFileTypeResources { get; set; }
    public virtual ICollection<Resource> StorageTypeResources { get; set; }
}