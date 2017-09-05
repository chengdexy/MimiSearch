using System;
using System.Net.Mail;
using System.Text;

namespace MimiSearch
{
    class MailSender
    {
        public string SendTo { get; set; } = "hi@chengdexy.cn";
        public string BodyHtml { get; set; }
        public bool IsHtml { get; set; } = true;

        public void Send(int imageCount)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(SendTo);
            msg.From = new MailAddress("xueyuanblog@163.com", "MimiSearcher", Encoding.UTF8);
            msg.Subject = DateTime.Now.ToString("yyyy-MM-dd")+$", {imageCount} images.";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.Body = BodyHtml;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = IsHtml;
            msg.Priority = MailPriority.Normal;

            SmtpClient client = new SmtpClient
            {
                Credentials = new System.Net.NetworkCredential("xueyuanblog", "darkmoon1"),
                Host = "smtp.163.com"
            };
            client.Send(msg);
        }

    }
}
