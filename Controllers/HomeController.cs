using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
                try
                {
                    Databases = DatabaseHelper.GetDatabaseNames(user.MachineName, user.UserName, user.Password);
                    ViewData["Databases"] = Databases;
                    ViewData["DatabaseName"] = user.MachineName;
                    return View();
                }
                catch (Exception e)
                {
                    TempData["Error"] = e.Message;
                    return Redirect("/Home/Login");
                }
            }
        }

        [HttpPost]
        public IActionResult Index(string sqlQuery, string databaseName)
        {
            try
            {
                ViewData["Query"] = sqlQuery;
                ViewData["DatabaseName"] = user.MachineName;
                ViewData["Columns"] = DatabaseHelper.GetColumnNames(user, sqlQuery, databaseName);
                ViewData["Rows"] = DatabaseHelper.Query(user, sqlQuery, databaseName);
                return View("Result");
            }
            catch (Exception e)
            {
                ViewData["DatabaseName"] = user.MachineName;
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
        public FileResult Save(string sqlQuery, string fileName)
        {
            if (!fileName.EndsWith(".sql"))
            {
                fileName += ".sql";
            }
            StreamWriter sw = null;
            using (sw = new StreamWriter(@".\wwwroot\download\" + fileName, false, Encoding.UTF8))
            {
                sw.Write(sqlQuery);
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(@".\wwwroot\download\" + fileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public string Upload(IFormFile sqlFile)
        {
            string line = "";
            string fileName = sqlFile.FileName;
            Stream s = sqlFile.OpenReadStream();
            string content = "";
            StreamReader sr = null;
            using (sr = new StreamReader(s, Encoding.UTF8))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    content += line + "\n";
                }
            }

            return $"{content}";
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
