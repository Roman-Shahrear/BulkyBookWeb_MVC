//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.Mvc.Formatters;
//using Microsoft.Extensions.Configuration;
//using SendGrid;
//using SendGrid.Helpers.Mail;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BulkyBook.Utility
//{
//    public class EmailSender : IEmailSender
//    {
//        public string SendGridSecret { get; set; }
//        public EmailSender(IConfiguration _config)
//        {
//            SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
//        }
//        public Task SendEmailAsync(string email, string subject, string htmlMessage)
//        {
//            //logic for send email
//            var client = new SendGridClient(SendGridSecret);

//            var from = new EmailAddress("theostimothyy@gmail.com", "Bulky Book");
//            var to = new EmailAddress(email);
//            var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

//            return client.SendEmailAsync(message);
//        }
//    }
//}


using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();

            // Sender Email Address (Gmail)
            emailMessage.From.Add(new MailboxAddress("Bulky Book", _config["EmailSettings:SenderEmail"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            // HTML message content
            var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_config["EmailSettings:SenderEmail"], _config["EmailSettings:SenderPassword"]);
                    await client.SendAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred while sending email: {ex.Message}");
                    throw; // Rethrow exception after logging
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}