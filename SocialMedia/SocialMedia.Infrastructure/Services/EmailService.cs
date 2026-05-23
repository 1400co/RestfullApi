using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.CustomEntities;

namespace SocialMedia.Infrastructure.Services
{
    public class EmailService(IOptions<SmtpSettings> smtpSettings) : IEmailService
    {

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", smtpSettings.Value.Username));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(isHtml ? "html" : "plain") { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpSettings.Value.Server, smtpSettings.Value.Port, smtpSettings.Value.UseSSL).ConfigureAwait(false);
                await client.AuthenticateAsync(smtpSettings.Value.Username, smtpSettings.Value.Password).ConfigureAwait(false);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
