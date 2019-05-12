using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TaskManager.Extensions.Email
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public EmailSettings _emailSettings { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(_emailSettings.UsernameEmail, "TaskManager")
            };

            mail.To.Add(email);

            mail.Subject = "Notification from your favourite web site - " + subject;
            mail.Body = message;
            mail.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient(_emailSettings.SecondayDomain, _emailSettings.SecondaryPort)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword),
                EnableSsl = true
            };

            return smtp.SendMailAsync(mail);
        }
    }
}
