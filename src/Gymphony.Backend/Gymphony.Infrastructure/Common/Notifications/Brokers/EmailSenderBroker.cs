using System.Net;
using System.Net.Mail;
using Gymphony.Application.Common.Notifications.Brokers;
using Gymphony.Application.Common.Notifications.Models.Dtos;
using Gymphony.Application.Common.Notifications.Models.Settings;
using Microsoft.Extensions.Options;

namespace Gymphony.Infrastructure.Common.Notifications.Brokers;

public class EmailSenderBroker(IOptions<SmtpEmailSenderSettings> smtpEmailSettings) : IEmailSenderBroker
{
    private readonly SmtpEmailSenderSettings _smtpEmailSenderSettings = smtpEmailSettings.Value;
    
    public bool Send(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        var mail = new MailMessage(_smtpEmailSenderSettings.CredentialAddress, message.Recipient.EmailAddress);
        mail.Subject = message.Title.ToString();
        mail.Body = message.Content.ToString();
        mail.IsBodyHtml = true;

        var smtpClient = new SmtpClient(_smtpEmailSenderSettings.Host, _smtpEmailSenderSettings.Port);
        smtpClient.Credentials =
            new NetworkCredential(_smtpEmailSenderSettings.CredentialAddress, _smtpEmailSenderSettings.Password);
        smtpClient.EnableSsl = true;

        try
        {
            smtpClient.Send(mail);
        }
        catch(Exception ex)
        {
            message.ErrorMessage = ex.Message;
            
            return false;
        }

        return true;
    }
}