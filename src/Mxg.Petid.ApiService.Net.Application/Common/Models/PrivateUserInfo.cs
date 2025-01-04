namespace Mxg.Petid.ApiService.Net.Application.Common.Models;

public class PrivateUserInfo
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? Birthdate { get; set; }
    public Guid? CityId { get; set; }
    public Guid? GenderTypeId { get; set; }
}