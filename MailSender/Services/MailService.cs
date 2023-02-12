using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
namespace MailSender.Services
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RNET101", _configuration["Mail:Username"]));
            foreach (var to in tos)
                message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            // add the body of the message
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = body;
            message.Body = bodyBuilder.ToMessageBody();

            // send the email using SMTP
            using (var client = new SmtpClient())
            {
                client.Connect(_configuration["Mail:Host"], 465, true);
                client.Authenticate(_configuration["Mail:Username"], _configuration["Mail:Password"]);
                client.Send(message);
                client.Disconnect(true);
            }
            //var mail = new MimeMessage();
            //mail.IsBodyHtml = isBodyHtml;
            //foreach (var to in tos)
            //    mail.To.Add(to);
            //mail.Subject = subject;
            //mail.Body = body;
            //mail.From = new(_configuration["Mail:Username"], "NG E-Ticaret", System.Text.Encoding.UTF8);

            //SmtpClient smtp = new();
            //smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            //smtp.Port = 995;
            //smtp.EnableSsl = true;
            //smtp.Host = _configuration["Mail:Host"];
            //await smtp.SendMailAsync(mail);
        }
    }
}
