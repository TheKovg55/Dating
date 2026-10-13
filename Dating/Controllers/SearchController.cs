using Dating.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dating.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search (single action handles optional query param to avoid ambiguous action matches)
        public ActionResult Search(string username = null, int? minAge = null, int? maxAge = null, string programmingLanguage = null)
        {
            // Ensure that the user is logged in
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            LoadProgrammingLanguages();

            string currentUsername = Session["Username"].ToString();
            string path = Server.MapPath("~/App_Data/Users.txt");

            List<Register> userList = new List<Register>();

            if (System.IO.File.Exists(path))
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue; // Skip empty lines
                    }

                    Register user = new Register();
                    string[] fields = line.Split(',');
                    foreach (string field in fields)
                    {
                        string[] keyValue = field.Split(':');
                        if (keyValue.Length == 2)
                        {
                            string key = keyValue[0].Trim();
                            string value = keyValue[1].Trim();
                            switch (key)
                            {
                                case "Firstname":
                                    user.FirstName = value;
                                    break;
                                case "Lastname":
                                    user.LastName = value;
                                    break;
                                case "Age":
                                    int.TryParse(value, out int age);
                                    user.Age = age;
                                    break;
                                case "Programming Language":
                                    user.ProgrammingLanguage = value;
                                    break;
                                case "Username":
                                    user.Username = value;
                                    break;
                            }
                        }
                    }

                    // Exclude current user
                    if (!string.Equals(user.Username, currentUsername, StringComparison.OrdinalIgnoreCase))
                    {
                        userList.Add(user);
                    }
                }
            }

            // If a search term was provided, filter the list
            if (!string.IsNullOrWhiteSpace(username))
            {
                userList = userList.Where(u =>
                    (u.FirstName != null && u.FirstName.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (u.LastName != null && u.LastName.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (u.Username != null && u.Username.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            }

            // Filter by age range if provided
            if (minAge.HasValue)
            {
                userList = userList.Where(u => u.Age >= minAge.Value).ToList();
            }
            if (maxAge.HasValue)
            {
                userList = userList.Where(u => u.Age <= maxAge.Value).ToList();
            }

            //filter by programming language if provided
            if(!string.IsNullOrWhiteSpace(programmingLanguage))
            {
                userList = userList.Where(u => string.Equals(u.ProgrammingLanguage, programmingLanguage, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            return View(userList);
        }

        public void LoadProgrammingLanguages()
        {
            string[] programmingLanguages = new string[] { "C#", "Java", "Python", "JavaScript", "Ruby", "Go", "Rust", "Swift" };
            ViewBag.ProgrammingLanguages = programmingLanguages;
        }
    }
}