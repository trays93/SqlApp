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
        private static List<string> Databases = new List<string>();
        private static List<Database> DatabaseTree = new List<Database>();

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
                Databases = DatabaseHelper.GetDatabaseNames(user.MachineName, user.UserName, user.Password);
                ViewData["Databases"] = Databases;
                //DatabaseTree = DatabaseHelper.MakeDatabaseTree(user);
                //ViewData["DatabaseTree"] = DatabaseTree;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Index(string sqlQuery, string database)
        {
            try
            {
                ViewData["Query"] = sqlQuery;
                ViewData["Columns"] = DatabaseHelper.GetColumnNames(user, sqlQuery, database);
                ViewData["Rows"] = DatabaseHelper.Query(user, sqlQuery, database);
                return View("Result");
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                ViewData["Databases"] = Databases;
                return View("Index");
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
