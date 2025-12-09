using System.Net;
using System.Net.Mail;

namespace backendMpact.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                // Read settings
                string smtpHost = _config["EmailSettings:SmtpHost"];
                string smtpPortStr = _config["EmailSettings:SmtpPort"];
                string fromEmail = _config["EmailSettings:FromEmail"];
                string fromName = _config["EmailSettings:FromName"];
                string smtpPass = _config["EmailSettings:SmtpPass"];

                Console.WriteLine($"[DEBUG] Loaded SMTP config:");
                Console.WriteLine($"Host: {smtpHost}");
                Console.WriteLine($"Port: {smtpPortStr}");
                Console.WriteLine($"FromEmail: {fromEmail}");
                Console.WriteLine($"FromName: {fromName}");
                Console.WriteLine($"App Password (length only): {smtpPass?.Length}");

                // Validate config
                if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(smtpPass))
                {
                    Console.WriteLine("[ERROR] Missing FromEmail OR SmtpPass in appsettings.json");
                    return false;
                }

                if (!int.TryParse(smtpPortStr, out int smtpPort))
                    smtpPort = 587; // default Gmail port

                var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(fromEmail, smtpPass),
                    EnableSsl = true,  // Gmail requires SSL
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 15000
                };

                var msg = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };

                msg.To.Add(to);
                msg.ReplyToList.Add(new MailAddress(fromEmail));

                Console.WriteLine($"[DEBUG] Attempting to send email...");

                await client.SendMailAsync(msg);

                Console.WriteLine("[SUCCESS] Email sent successfully!");
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"[SMTP ERROR] Status: {ex.StatusCode}");
                Console.WriteLine($"[SMTP ERROR] Message: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[SMTP INNER] {ex.InnerException.Message}");

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Exception when sending email: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("[INNER EXCEPTION] " + ex.InnerException.Message);

                return false;
            }
        }
    }
}
