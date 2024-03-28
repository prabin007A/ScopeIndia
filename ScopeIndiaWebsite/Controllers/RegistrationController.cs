using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using ScopeIndiaWebsite.Models;
using ScopeIndiaWebsite.Entity;
using System.Linq;

namespace ScopeIndia.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ScopeDbContext dbContext;

        public RegistrationController(ScopeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public List<string> Hobbies { get; set; }
        string hobbie;

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(Student st, IFormFile myfile)
        {
            var a = dbContext.Student.FirstOrDefault(x => x.Email == st.Email);
            if (a != null)
            {
                ViewBag.MS = "This email id was already registered";
                return View("Registration");
            }

            if (Hobbies != null && Hobbies.Count > 0)
            {
                foreach (var item in Hobbies)
                {
                    hobbie += item + ",";
                }
            }

            string file_name = myfile.FileName;
            file_name = Path.GetFileName(file_name);
            string upload_folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(upload_folder))
            {
                Directory.CreateDirectory(upload_folder);
            }

            string upload_path = Path.Combine(upload_folder, file_name);

            if (System.IO.File.Exists(upload_path))
            {
                ViewBag.UploadStatus += file_name + " Already exists";
                Random file_number = new Random();
                file_name = file_number.Next().ToString() + file_name;
                upload_path = Path.Combine(upload_folder, file_name);
            }
            else
            {
                ViewBag.UploadStatus += file_name + "Uploaded successfully";
            }

            using (var upload_steam = new FileStream(upload_path, FileMode.Create))
            {
                myfile.CopyTo(upload_steam);
            }

            string path = "~/uploads/";

            // Using Entity Framework
            var stud = new Students
            {
                First_name = st.First_name,
                Last_name = st.Last_name,
                Gender = st.Gender,
                DateOfBirth = st.DateOfBirth,
                Email = st.Email,
                Phone_number = st.Phone_number,
                Country = st.Country,
                State = st.State,
                City = st.City,
                Hobbie = hobbie,
                Avatar = path + file_name,
            };
            dbContext.Student.Add(stud);
            dbContext.SaveChanges();

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("prabinraj48@gmail.com");
            mail.To.Add(new MailAddress(st.Email));
            mail.Subject = "Confirmation message";
            mail.IsBodyHtml = true;
            mail.Body = "You have successfully registered with Scope INDIA";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("prabinraj48@gmail.com", "wwmu libo dyrz fhzt");
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(mail);
                ViewBag.success = "Registered successfully...!";
            }
            catch (Exception)
            {
                ViewBag.failed = "Unable to register now, please try again later...!";
            }

            var return_page = "Message";
            return View(return_page);
        }
    }
}
