using Mxg.Petid.ApiService.Net.Application.Common.Models.Email;

namespace Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Infraestructure;

public interface IEmailService
{
    Task<bool> SendEmail(Email email);
}