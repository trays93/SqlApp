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
                return View();
            }
        }

        [HttpPost]
        public IActionResult Index(string sqlQuery, string databaseName)
        {
            try
            {
                ViewData["Query"] = sqlQuery;
                ViewData["Columns"] = DatabaseHelper.GetColumnNames(user, sqlQuery, databaseName);
                ViewData["Rows"] = DatabaseHelper.Query(user, sqlQuery, databaseName);
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

        [HttpPost]
        public string Save(string sqlQuery, string fileName)
        {
            return $"SQL: {sqlQuery}, <br>Fájlnév: {fileName}";
        }

        [HttpPost]
        public IActionResult GetTables(string databaseName)
        {
            List<string> tables = DatabaseHelper.GetTables(user, databaseName);
            return new JsonResult(tables);
        }

        [HttpPost]
        public IActionResult GetColumns(string databaseName, string tableName)
        {
            List<string> columns = DatabaseHelper.GetColumns(user, databaseName, tableName);
            return new JsonResult(columns);
        }
    }
}
