using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace E_ticket.utiltiy
{
    public class EmailSend : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("hsn768972@gmail.com", "fmvc gyjl rmsv rvqs")
            };
            return client.SendMailAsync(
                          new MailMessage(from: "hsn768972@gmail.com",
                                          to: email,
                                          subject,
                                          message
                                          )
                          {
                              IsBodyHtml = true
                          });


        }
    }
}
