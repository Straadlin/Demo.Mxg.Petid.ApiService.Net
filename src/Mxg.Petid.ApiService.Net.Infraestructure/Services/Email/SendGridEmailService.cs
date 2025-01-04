using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Infraestructure;
using Mxg.Petid.ApiService.Net.Application.Common.Models.Email;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Services.Email;

public class SendGridEmailService : IEmailService
{
    public EmailSettings EmailSettings { get; }
    public ILogger<SendGridEmailService> Logger { get; }

    public SendGridEmailService(IOptions<EmailSettings> emailSettings, ILogger<SendGridEmailService> logger)
    {
        EmailSettings = emailSettings.Value;
        Logger = logger;
    }

    public async Task<bool> SendEmail(Application.Common.Models.Email.Email email)
    {
        var client = new SendGridClient(EmailSettings.ApiKey);

        var subject = email.Subject;
        var to = new EmailAddress(email.To);
        var emailBody = email.Body;

        var from = new EmailAddress
        {
            Email = EmailSettings.FromAddress,
            Name = EmailSettings.FromName
        };

        var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
        var response = await client.SendEmailAsync(sendGridMessage);

        if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return true;
        }

        Logger.LogError("Email couldn't be sent, there are any errors.");
        return false;
    }
}