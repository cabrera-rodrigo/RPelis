using APP_PELIS.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using static System.Net.Mime.MediaTypeNames;


namespace APP_PELIS.Service
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string htmlBody, string? textBody = null);
        string MensajeBienvenida(string nombreUsuario);
    }
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpSettings _cfg;
        public SmtpEmailService(IOptions<SmtpSettings> cfg) => _cfg = cfg.Value;

        public async Task SendAsync(string to, string subject, string htmlBody, string? textBody = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_cfg.FromName, _cfg.User));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            

            var builder = new BodyBuilder { HtmlBody = htmlBody, TextBody = textBody ?? string.Empty };
            message.Body = builder.ToMessageBody();
            

            using var client = new SmtpClient();
            await client.ConnectAsync(_cfg.Host, _cfg.Port, _cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto);
            await client.AuthenticateAsync(_cfg.User, _cfg.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        public sealed class SmtpSettings
        {
            public string Host { get; set; } = "";
            public int Port { get; set; }
            public string User { get; set; } = "";
            public string Password { get; set; } = "";
            public string FromName { get; set; } = "Sistema";
            public bool UseStartTls { get; set; } = true;
        }
        public string MensajeBienvenida(string nombreUsuario)
        {
            return $"""
            <html>
            <body style="background-color: #f4f4f4; padding: 20px;">
                <div style="background-color: white; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #dddddd; border-radius: 10px; font-family: Arial, sans-serif;">
                    <h1 style="color: #e50914; text-align: center;">¡Bienvenido a Rpelis!</h1>
                    <p style="font-size: 16px; color: #333333;">Hola <strong>{nombreUsuario}</strong>,</p>
                    <p style="font-size: 14px; color: #666666;">Estamos muy felices de que te hayas registrado. Ahora puedes calificar tus películas favoritas.</p>
                    <div style="text-align: center; margin-top: 25px;">
                        <a href="http://rpelis.runasp.net/" style="background-color: #e50914; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; font-weight: bold;">Explorar Catálogo</a>
                    </div>
                    <p style="font-size: 12px; color: #999999; margin-top: 30px; border-top: 1px solid #eeeeee; padding-top: 10px; text-align: center;">
                        © 2026 Rpelis - Plataforma de peliculas
                    </p>
                </div>
            </body>
            </html>
            """;

        }
    }
}
