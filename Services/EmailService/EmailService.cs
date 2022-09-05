using Microsoft.AspNetCore.Mvc;
using StajyerTakip.Models;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using MailKit.Net.Smtp;
using SmtpClient = System.Net.Mail.SmtpClient;
using System.Net.Mime;
namespace StajyerTakip.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void Send([Bind("To,Subject,Body,FromEmail,FromPassword")] MailMessageModel mail)
        {
            using (MailMessage mailMessage = new MailMessage(_config.GetSection("EmailUserName").Value, mail.To))
            {
                mailMessage.Subject = mail.Subject;
                mailMessage.Body = mail.Body;
                mailMessage.IsBodyHtml = false;

              

                System.IO.DirectoryInfo di = new DirectoryInfo("wwwroot\\BasvuruFiles");

                foreach (FileInfo f in di.EnumerateFiles())
                {
                    var file = f.FullName;
                    Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                    // Add time stamp information for the file.
                    System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(file);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                    // Add the file attachment to this email message.
                    mailMessage.Attachments.Add(data);
                }


                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = _config.GetSection("EmailHost").Value;
                    smtp.UseDefaultCredentials = false;
                    System.Net.NetworkCredential cred = new System.Net.NetworkCredential(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);

                    smtp.EnableSsl = true;
                    smtp.Credentials = cred;
                    smtp.Port = 587;
                    try
                    {
                        smtp.Send(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                            ex.ToString());
                    }
                }
            }
        }
    }
}
