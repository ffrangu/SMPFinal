using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SMP.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _hostingEnvironment;

        public EmailSender(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                NetworkCredential basicCredential = new NetworkCredential(_configuration.GetSection("appSettings").GetValue<string>("SmtpUser"), _configuration.GetSection("appSettings").GetValue<string>("SmtpPwd"));
                MailMessage mailMessage = new MailMessage();
                MailAddress fromAddress = new MailAddress(_configuration.GetSection("appSettings").GetValue<string>("EmailFrom"));
                smtpClient.Host = _configuration.GetSection("appSettings").GetValue<string>("SmtpServer");
                smtpClient.Port = _configuration.GetSection("appSettings").GetValue<int>("SmtpPort");
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                smtpClient.EnableSsl = true;
                mailMessage.From = fromAddress;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = BodyContent(htmlMessage);
                mailMessage.To.Add(email);
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch { }
        }

        private string BodyContent(string mainContent)
        {
            var image = Path.Combine(_hostingEnvironment.WebRootPath, "Media", "Images", "LogoFinal.png");

            string header = "<html>" +
                "<body>" +
                    "<header> <h4>Hello,</h4> </header>" +
                    "{0}" +
                    "<footer>" +
                    "<h4>SMP" +
                    "</h4>" +
                    "<span>" +
                    "<img src = '{1}' width='128' height='30' /></span>" +
                    "</footer>" +
                "</body>" +
                "</html>";

            MemoryStream ms = new MemoryStream();
            return string.Format(header, mainContent, image);
        }
    }
}
