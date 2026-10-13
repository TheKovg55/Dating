using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dating.Models;
namespace Dating.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {

            //Loading the programming languages in the view when the page is loaded
            LoadProgrammingLanguages();
            //putting the programming languages in a string array to be used in the view
            string[] programmingLanguages = new string[] { "C#", "Java", "Python", "JavaScript", "Ruby", "Go", "Rust", "Swift" };
            ViewBag.ProgrammingLanguages = programmingLanguages;
            return View();
        }


        [HttpPost]
        public ActionResult Index(Register user)
        {
            //Loading the programming languages in the view after the form is submitted
            LoadProgrammingLanguages();
            //checking condition if the model already exists in the text file
            StreamReader reader = new StreamReader(Server.MapPath("~/App_Data/Users.txt"));
            if (reader != null)
            {
                string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/App_Data/Users.txt"));
                reader.Close();
                foreach (string line in lines)
                {
                    if (line.Contains(user.Username))
                    {
                        ViewBag.AlertMessage = "Username already exists. Please choose a different username.";
                        return View(user);
                    }
                    if (!line.Contains(user.Username) && !line.Contains(user.Password))
                    {
                        ViewBag.AlertMessage = "Username and password are valid.";
                        SaveData(user);
                        return View(user);
                    }

                }
            }
            //calling streamwriter to save the user information in a text file
            StreamWriter Save = new StreamWriter(Server.MapPath("~/App_Data/Users.txt"), true);
            Save.WriteLine($"Firstname: {user.FirstName}, Lastname: {user.LastName}, Age: {user.Age}, Programming Language: {user.ProgrammingLanguage}, Username: {user.Username}, Password: {user.Password}");
            Save.Close();




            //refreshing the programming languages in the view after the form is submitted
            /*string[] programmingLanguages = new string[] { "C#", "Java", "Python", "JavaScript", "Ruby", "Go", "Rust", "Swift" };
            ViewBag.ProgrammingLanguages = programmingLanguages;*/
            return View();
        }

        public void LoadProgrammingLanguages()
        {
            string[] programmingLanguages = new string[] { "C#", "Java", "Python", "JavaScript", "Ruby", "Go", "Rust", "Swift" };
            ViewBag.ProgrammingLanguages = programmingLanguages;
        }

        public void SaveData(Register user)
        {
            StreamWriter Save = new StreamWriter(Server.MapPath("~/App_Data/Users.txt"), true);
            Save.WriteLine($"Firstname: {user.FirstName}, Lastname: {user.LastName}, Age: {user.Age}, Programming Language: {user.ProgrammingLanguage}, Username: {user.Username}, Password: {user.Password}");
            Save.Close();
        }
    }
}