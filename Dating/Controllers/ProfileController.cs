using Dating.Models;
using System;
using System.IO;
using System.Web.Mvc;

namespace Dating.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Profile()
        {
            // check if session variable is null
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //reloading the programming languages to the view
            LoadProgrammingLanguages();

            // get the username from the session variable
            string username = Session["Username"].ToString();

            // create the user object
            Register user = new Register();

            // We read the file now:
            string filePath = Server.MapPath("~/App_Data/Users.txt");
            if (System.IO.File.Exists(filePath))
            {
                StreamReader reader = new StreamReader(filePath);
                string line;

                // Read the file line by line
                while ((line = reader.ReadLine()) != null)
                {
                    // 1. FIRST check if this line belongs to the logged-in user
                    if (line.Contains($"Username: {username},"))
                    {
                        // Split the line into fields
                        string[] fields = line.Split(',');

                        // splitting the fields into key value pairs
                        foreach (string field in fields)
                        {
                            string[] keyValue = field.Split(':');

                            if (keyValue.Length == 2)
                            {
                                string key = keyValue[0].Trim();
                                string value = keyValue[1].Trim();

                                // 2. Keys match the exact casing used in StreamWriter ("Firstname", "Lastname", "Programming Language")
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
                                    case "Password":
                                        user.Password = value;
                                        break;
                                }
                            }
                        }

                        // 3. Exit the loop ONLY after finding and parsing the correct user!
                        break;
                    }
                }
                reader.Close();
            }

            // return the user to the view
            return View(user);
        }

        // logout method
        public ActionResult Logout()
        {
            // clear the session variable
            Session.Clear();
            Session.Abandon();

            // redirect to the login page
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Profile(Register UpdatedUser)
        {   

            //Check if the session variable is still valid

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            //reloading the programming languages to the view
            LoadProgrammingLanguages();

            //store the username and filepath in a variable to save typing it multiple times
            string username = Session["Username"].ToString();  
            string path = Server.MapPath("~/App_Data/Users.txt");

            if(System.IO.File.Exists(path))
            {
                //Reading all the lines from the file
                string[] lines = System.IO.File.ReadAllLines(path);

                //The code to insert into the text file:
                string newLine = $"Firstname: {UpdatedUser.FirstName}, Lastname: {UpdatedUser.LastName}, Age: {UpdatedUser.Age}, Programming Language: {UpdatedUser.ProgrammingLanguage}, Username: {UpdatedUser.Username}, Password: {UpdatedUser.Password}";

                //Using a loop to check through the entire document for the username and replace it with the new line:
                for (int i  = 0; i  < lines.Length; i ++)
                {
                    if (lines[i].Contains($"Username: {username},")){
                        lines[i] = newLine;
                        break;
                    }
                }

                //This is to rewrite the entire file with the updated line:
                System.IO.File.WriteAllLines(path, lines);

                //updating the username
                Session["Username"] = UpdatedUser.Username;
                ViewBag.UpdateMessage = "Profile updated successfully!";

            }

            return View(UpdatedUser);



        }

        public void LoadProgrammingLanguages()
        {
            string[] programmingLanguages = new string[] { "C#", "Java", "Python", "JavaScript", "Ruby", "Go", "Rust", "Swift" };
            ViewBag.ProgrammingLanguages = programmingLanguages;
        }
    }
}