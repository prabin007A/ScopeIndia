using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace ScopeIndiaWebsite.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(string Name, string Email, string Subject, string Message)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("prabinraj48@gmail.com");
            mail.To.Add(new MailAddress("prabinraj48@gmail.com"));
            mail.Subject = "Student information";
            mail.IsBodyHtml = false;
            mail.Body = $"Name : {Name}\nEmail Address : {Email}\nSubject : {Subject}\nMessage : {Message}";
            using var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("prabinraj48@gmail.com", "wwmu libo dyrz fhzt");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
                ViewBag.success = "Mail sented successfully...!We will contat you soon...!";
            }
            catch (Exception)
            {
                ViewBag.failed = "Unable to send an email right now please try again....!";
            }
            ViewBag.Name = Name;
            ViewBag.Email = Email;
            ViewBag.Subject = Subject;
            ViewBag.Message = Message;
            var return_page = "Contact";
            if (ModelState.IsValid)
            {
                return_page = "Email";
            }
            return View(return_page);
        }
    }
}
