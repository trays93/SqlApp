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
        private User user = new User();

        [HttpGet]
        public object Index()
        {
            return View();
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
            return View("Index");
        }
    }
}
