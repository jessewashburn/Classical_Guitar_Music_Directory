using CGMD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

public class MailController : Controller
{
    [HttpPost]
    public IActionResult Mail(ContactFormModel model)
    {
        /*
        // Set the Gmail SMTP host and port
        using (var client = new SmtpClient("smtp.gmail.com", 465))
        {
            // Specify your Gmail SMTP host and port above
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("your-email@gmail.com", "your-app-password"); // Replace with your Gmail email and App Password
            client.EnableSsl = true; // Use SSL/TLS for secure connection

            var message = new MailMessage
            {
                From = new MailAddress("your-email@gmail.com"), // Replace with your Gmail email
                Subject = model.SubjectLine,
                Body = model.Message
            };

            message.To.Add("jessewashburn.guitar@gmail.com");

            client.Send(message);
        }
        */
        // Redirect to the confirmation page
        return View("~/Views/Home/EmailConf.cshtml");
    }

    [HttpPost]
    public IActionResult Post(ContactFormModel model)
    {
        /*
        // Set the Gmail SMTP host and port
        using (var client = new SmtpClient("smtp.gmail.com", 465))
        {
            // Specify your Gmail SMTP host and port above
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("your-email@gmail.com", "your-app-password"); // Replace with your Gmail email and App Password
            client.EnableSsl = true; // Use SSL/TLS for secure connection

            var message = new MailMessage
            {
                From = new MailAddress("your-email@gmail.com"), // Replace with your Gmail email
                Subject = model.SubjectLine,
                Body = model.Message
            };

            message.To.Add("jessewashburn.guitar@gmail.com");

            client.Send(message);
        }
        */
        // Redirect to the confirmation page
        return View("~/Views/Home/PostConf.cshtml");
    }

}
