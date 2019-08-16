using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginReggy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace LoginReggy.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/register")]
        public IActionResult Register(UserReg newUserForm)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.UserRegs.Any(u => u.Email == newUserForm.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }

                PasswordHasher<UserReg> Hasher = new PasswordHasher<UserReg>();
                newUserForm.Password = Hasher.HashPassword(newUserForm, newUserForm.Password);
                dbContext.Add(newUserForm);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserSessId", newUserForm.UserId);
                return RedirectToAction("Success");
            }
            else{
                return View("Index");
            }
        }

        [HttpGet("login")]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost("verify")]
        public IActionResult VerifyLogin(UserLog loginForm)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.UserRegs.FirstOrDefault(u => u.Email == loginForm.Email);

                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("LoginPage");
                }

                var hasher = new PasswordHasher<UserLog>();
                var result = hasher.VerifyHashedPassword(loginForm, userInDb.Password, loginForm.Password);

                if(result ==0)
                {
                    ModelState.AddModelError("Password", "Invalid Email/Password");
                    return View("LoginPage");
                }
                
                HttpContext.Session.SetInt32("UserSessId", userInDb.UserId);
                int? abc = HttpContext.Session.GetInt32("UserSessId");
                return RedirectToAction("Success");
            }
            else{

                return View("LoginPage");
            }
        }
        
        [HttpGet("success")]
        public IActionResult Success()
        {
            int? UserSess = HttpContext.Session.GetInt32("UserSessId");
            if(HttpContext.Session.GetInt32("UserSessId")== 0)
            {
                return View("LoginPage");
            }
            if(HttpContext.Session.GetInt32("UserSessId")== null)
            {
                return View("LoginPage");
            }
            
                return View("Success");

        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            int? testing = HttpContext.Session.GetInt32("UserSessId");
            System.Console.WriteLine(testing+"Testing");
            return RedirectToAction("Index");
        } 
            
            


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
