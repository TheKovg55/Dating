using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dating.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        { 
            //checking condition if the model already exists in the text file
            StreamReader reader = new StreamReader(Server.MapPath("~/App_Data/Users.txt"));
            if (reader != null)
            {
                string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/App_Data/Users.txt"));
                reader.Close();
                foreach (string line in lines)
                {
                    if (line.Contains(username) && line.Contains(password))
                    {
                        //we store the variable in the session
                        Session["Username"] = username;
                        //we redirect the user to the profile page after successful login
                        return RedirectToAction("Profile", "Profile");
                    }
                }
            }
            ViewBag.LoginMessage = "Invalid username or password. Please try again.";
            return View();
        }
    }
}