using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ChoKawo.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration configuration) {
            _config = configuration;
        }

        public async Task SendEmailAsync(string email, string senderName, string message)
        {
            var eMessage = new MimeMessage();
            var emailTo = _config["email:to"];
            var emailProvider = _config["email:provider"];
            var emailPass = _config["email:pass"];
            var emailUser = _config["email:user"];

            var composedMessage = $"{senderName}\n{email}\n{message}";

            eMessage.From.Add(new MailboxAddress("New Request", emailUser));
            eMessage.To.Add(new MailboxAddress("", emailTo));
            eMessage.Subject = senderName;
            
            eMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = composedMessage
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(emailProvider, 25, false);
            await client.AuthenticateAsync(emailUser, emailPass);
            await client.SendAsync(eMessage);

            await client.DisconnectAsync(true);

        }
    }
}
