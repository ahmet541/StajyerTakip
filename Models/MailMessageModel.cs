using System.ComponentModel.DataAnnotations;

namespace StajyerTakip.Models
{
    public class MailMessageModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FromEmail { get; set; }
        [DataType(DataType.Password)]
        public string FromPassword { get; set; }

        public MailMessageModel(string to, string subject, string body, string fromEmail, string fromPassword)
        {
            To = to;
            Subject = subject;
            Body = body;
            FromEmail = fromEmail;
            FromPassword = fromPassword;
        }

        public MailMessageModel()
        {
            To = "";
            Subject = "";
            Body = "";
            FromEmail = "";
            FromPassword = "";

        }
    }
}
