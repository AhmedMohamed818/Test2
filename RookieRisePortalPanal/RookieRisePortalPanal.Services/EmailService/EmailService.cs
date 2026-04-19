using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using RookieRisePortalPanal.Services.AppConfigration;
using RookieRisePortalPanal.Services.EmailService.DTO;
using System.Net;
using System.Net.Mail;

namespace RookieRisePortalPanal.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtp;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<SmtpSettings> smtp,
            ILogger<EmailService> logger)
        {
            _smtp = smtp.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(SendEmailDto emailDto)
        {
            try
            {
                _logger.LogInformation("Sending email to {Email}", emailDto.To);

                using var client = new SmtpClient(_smtp.Host)
                {
                    Port = _smtp.Port,
                    Credentials = new NetworkCredential(_smtp.Email, _smtp.Password),
                    EnableSsl = true
                };

                using var mail = new MailMessage
                {
                    From = new MailAddress(_smtp.Email),
                    Subject = emailDto.Subject,
                    Body = emailDto.Body,
                };

                mail.To.Add(emailDto.To);

                await client.SendMailAsync(mail);

                _logger.LogInformation("Email sent successfully to {Email}", emailDto.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Email}", emailDto.To);
                throw;
            }
        }
    }
}