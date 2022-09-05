using Microsoft.AspNetCore.Mvc;
using StajyerTakip.Models;

namespace StajyerTakip.Services.EmailService
{
    public interface IEmailService
    {
        void Send([Bind("To,Subject,Body,FromEmail,FromPassword")] MailMessageModel mail);
    }
}
