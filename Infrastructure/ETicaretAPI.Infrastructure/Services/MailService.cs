using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Infrastructure.Services
{
    public class MailService:IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[]{to},subject,body,isBodyHtml);

        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos) 
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["Mail:Username"], "Mini E-Ticaret", System.Text.Encoding.UTF8);
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);
        }


        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {//Bu metodu Chat - GPT ye düzelttirdik, bizim metod altta link oluşmuyor metod hata veriyordu.
            StringBuilder mail = new();
            mail.Append("Merhaba<br>");
            mail.Append("Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br>");
            mail.Append("<strong><a target=\"_blank\" href=\"");
            mail.Append(_configuration["AngularClientUrl"]);
            mail.Append("/update-password/");
            mail.Append(userId);
            mail.Append("/");
            mail.Append(resetToken);
            mail.Append("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br>");
            mail.AppendLine("<span satyle=\"font-size:12px;\" NOT :Eğer bu talep sizin tarafınızdan gönderilmediyse lütfen bu maili dikkate almayınız.></span><br>");
            mail.Append("Saygılarımızla...<br><br><br>RE Mini | E-Ticaret");

            // Oluşturulan HTML içeriğini debuglamak için içeriği konsola yazdırın
            string mailContent = mail.ToString();
            Console.WriteLine(mailContent);

            await SendMailAsync(to, "Şifre Yenileme Talebi", mailContent);
        }


        /*public async Task SendPasswordResetMailAsync(string to,string userId, string resetToken)
        {
            StringBuilder mail=new ();
            *//*mail.AppendLine("MErhamaba!<br>Eğer yeni bir şifre talebinde bulunduysanız aşağıdaki linke tıklayarak şifrenizi yenileyebilirsiniz.<br><strong><a target\"_blank\" href=\"....../userId/resetToken\"");*//*
            //mail.AppendLine("Merhaba!<br>Eğer yeni bir şifre talebinde bulunduysanız aşağıdaki linke tıklayarak şifrenizi yenileyebilirsiniz.<br><strong><a target\"_blank\" href=\"");
            mail.Append("Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br><strong><a target=\"_blank\" href=\"");
            mail.Append(_configuration["AngularClientUrl"]);
            mail.Append("/update-password/");
            mail.Append(userId);
            mail.Append("/");
            mail.Append(resetToken);
            mail.Append("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Eğer ki bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br>NG - Mini|E-Ticaret");
            //mail.AppendLine("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span satyle=\"font-size:12px;\" NOT :Eğer bu talep sizin tarafınızdan gönderilmediyse lütfen bu maili dikkate almayınız.></span><br>Saygılarımızla...<br><br><br>RE Mini|E-Ticaret");

            await SendMailAsync(to, "Şifre Yenileme Talebi", mail.ToString());
            
        }*/
    }
}
