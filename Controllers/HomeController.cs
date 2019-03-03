using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SQLApp.Models;

namespace SQLApp.Controllers
{
    public class HomeController : Controller
    {
        private static User user = new User();

        [HttpGet]
        public IActionResult Index()
        {
            if (user.MachineName == "" &&
                user.UserName == "" &&
                user.Password == "")
            {
                return Redirect("/Home/Login");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string machineName, string userName, string password)
        {
            user.MachineName = machineName;
            user.UserName = userName;
            user.Password = password;
            return Redirect("/Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            user.MachineName = "";
            user.UserName = "";
            user.Password = "";
            return Redirect("/Home");
        }
    }
}
