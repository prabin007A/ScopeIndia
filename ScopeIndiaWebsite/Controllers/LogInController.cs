using Microsoft.AspNetCore.Mvc;
using ScopeIndiaWebsite.Models;
using Microsoft.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using ScopeIndiaWebsite.Entity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Options;
using System.Collections.Generic;


namespace ScopeIndiaWebsite.Controllers
{
    public class LogInController : Controller
    {

        private readonly ScopeDbContext dbContext;
        public LogInController(ScopeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Login()
        {

            if (HttpContext.Request.Cookies["RememberMeCookie"] != null)
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Login");
            }
        }

        [HttpPost]
        public IActionResult Login(LoginModel ft)
        {
            CookieOptions opt = new CookieOptions();
            opt.Expires = DateTime.Now.AddHours(1);
            HttpContext.Response.Cookies.Append("Remember", ft.Email, opt);
                var check_login_details = dbContext.LoginDetails.FirstOrDefault(a => a.Email == ft.Email && a.Password == ft.Password);
                if (check_login_details != null)
                {
                    //for enable cookies
                    if (ft.CheckBox == true)
                    {
                        CookieOptions options = new CookieOptions();
                        options.Expires = DateTime.Now.AddHours(1);
                        HttpContext.Response.Cookies.Append("RememberMeCookie", ft.Email, options);
                        return RedirectToAction("DashBoard");
                    }
                    return RedirectToAction("DashBoard");
                }
                else
                {
                    ViewBag.fail = "Email and password dosn't matched";
                    return View("Login");
                }
        }

        public IActionResult FirstTimeLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult FirstTimeLogin(FirstTimeLogin frt)
        {
            CookieOptions a = new CookieOptions();
            a.Expires = DateTime.Now.AddHours(1);
            HttpContext.Response.Cookies.Append("MAIL", frt.Email, a);

            var check = dbContext.LoginDetails.FirstOrDefault(a => a.Email == frt.Email);
            if (check == null)
            {
                //To generate a random number

                Random random = new Random();
                var random_number = random.Next(1000, 9999);
                TempData["Email"] = frt.Email;
                var email1 = TempData["Email"];

                //To sent an otp to user

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("prabinraj48@gmail.com");
                mail.To.Add(new MailAddress(email1.ToString()));
                mail.Subject = "OTP";
                mail.IsBodyHtml = true;
                mail.Body = random_number + " Is your one time password for creating a new password";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("prabinraj48@gmail.com", "wwmu libo dyrz fhzt");
                smtp.EnableSsl = true;
                smtp.Send(mail);

                var log = new LogIn 
                { 
                    Email = frt.Email,
                    Password=random_number.ToString(),
                };
                dbContext.LoginDetails.Add(log);
                dbContext.SaveChanges();
                return RedirectToAction("Otp");
            }
            else
            {
                ViewBag.messa = "Email already exists try another email address";
            }
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(FirstTimeLogin fi)
        {
            var user = dbContext.LoginDetails.FirstOrDefault(a => a.Email == fi.Email);
            if (user != null)
            {
                // If email found, send an OTP to that email
                Random random = new Random();
                var random_number = random.Next(1000, 9999);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("prabinraj48@gmail.com");
                mail.To.Add(new MailAddress(fi.Email));
                mail.Subject = "OTP";
                mail.IsBodyHtml = true;
                mail.Body = random_number + " Is your one time password for creating a new password";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("prabinraj48@gmail.com", "wwmu libo dyrz fhzt");
                smtp.EnableSsl = true;
                smtp.Send(mail);

                // Update user's password in the database
                user.Password = random_number.ToString();
                dbContext.SaveChanges();

                // Redirect to OTP confirmation view
                return RedirectToAction("Otp");
            }
            else
            {
                // If email not found
                ViewBag.Mes = "Account not found! Create a new one...";
                return View("ForgotPassword");
            }
        }

        public IActionResult Otp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Otp(FirstTimeLogin firstTimeLogin)
        {
            // Find the user by password
            var user = dbContext.LoginDetails.FirstOrDefault(b =>b.Password == firstTimeLogin.Otp);

            if (user != null)
            {
                return RedirectToAction("PasswordCreation");
            }
            else
            {
                ViewBag.Failed = "Invalid OTP";
                return View("Otp");
            }
        }
        public IActionResult PasswordCreation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordCreation(FirstTimeLogin first)
        {
            var email = HttpContext.Request.Cookies["MAIL"];
            var update = dbContext.LoginDetails.FirstOrDefault(s => s.Email == email);

            if (update != null)
            {
                update.Password = first.Password;
                dbContext.SaveChanges();
                ViewBag.success = "Password updated successfully";
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Dashboard()
        {
            var email = HttpContext.Request.Cookies["Remember"];
            var user = dbContext.Student.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                ViewBag.Name = user.First_name;
                ViewBag.Lname=user.Last_name;
                ViewBag.Image = user.Avatar;
            }
            return View();
        }
        public IActionResult ExistingPassword()
        {
            return View();
        }
        [HttpPost]

        public IActionResult ExistingPassword(FirstTimeLogin firstTime)
        {
            var Mail= HttpContext.Request.Cookies["Remember"]; 
            var existing_pass =dbContext.LoginDetails.FirstOrDefault(x=>x.Email==Mail && x.Password==firstTime.Password);
            if (existing_pass != null)
            {
                return RedirectToAction("NewPassword");
            }
            else
            {
                ViewBag.MS = "incorrect password";
                return View(); 
            }
        }
        public IActionResult NewPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewPassword(FirstTimeLogin firstTimeLogin)
        {
            var email = HttpContext.Request.Cookies["Remember"];
            var update = dbContext.LoginDetails.FirstOrDefault(s => s.Email == email);

            if (update != null)
            {
                update.Password = firstTimeLogin.Password;
                dbContext.SaveChanges();
                ViewBag.update = "Password updated successfully";
                return RedirectToAction("Login");
            }
                return View("PasswordCreation");
        }

        public IActionResult ProfileEdit()
        {
            var email = HttpContext.Request.Cookies["Remember"];
            var select = dbContext.Student.Where(a => a.Email == email).ToList();

            if (select != null && select.Count > 0)
            {
                return View(select);
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult Update(int Id)
        {
            var stud = dbContext.Student.FirstOrDefault(s => s.Id == Id);
            if (stud != null)
            {
                var update = new StudentUpdateModel()
                {
                    Id = stud.Id,
                    First_name = stud.First_name,
                    Last_name = stud.Last_name,
                    Gender = stud.Gender,
                    DateOfBirth = stud.DateOfBirth,
                    Email = stud.Email,
                    Phone_number = stud.Phone_number,
                    Country = stud.Country,
                    State = stud.State,
                    City = stud.City,
                };
                return View(update);
            }
            return RedirectToAction("Update");
        }

        [HttpPost]
        public IActionResult Update(StudentUpdateModel std)
        {

            var student = dbContext.Student.Find(std.Id);
            if (student != null)
            {
                student.Id = std.Id; 
                student.First_name = std.First_name;
                student.Last_name = std.Last_name;
                student.Gender = std.Gender;
                student.DateOfBirth = std.DateOfBirth;
                student.Email = std.Email;
                student.Phone_number = std.Phone_number;
                student.Country = std.Country;
                student.State = std.State;
                student.City = std.City;
                dbContext.SaveChanges();
                return RedirectToAction("ProfileEdit");
            }
            return Redirect("ProfileEdit");
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("RememberMeCookie");
            Response.Cookies.Delete("Remember");
            return RedirectToAction("Login");
        }

    }
}
