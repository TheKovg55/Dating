using MySqlConnector;
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

            //Here we are going to use 'using' to not worry about having to do conn open and close

            string query = "Select Count(*) from Student where Username = @Username and Password = @Password ";

            //Using statement saves the hassle of doing conn.open and conn.close

            using (MySqlConnection Conn = MySqlConnector())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, Conn))
                {

                    //Inserting parameters
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    //Executing command

                    long count = (long)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        //we store the variable in the session
                        Session["Username"] = username;

                        return RedirectToAction("Profile", "Profile");
                    }




                }


            }


            ViewBag.LoginMessage = "Invalid username or password. Please try again.";
            return View();
        }


        //Mysqlconnector method
        public MySqlConnection MySqlConnector()
        {
            //This is the format to create a connection to the database.   
            MySqlConnection Conn = new MySqlConnection("server=localhost;user id=root;password=;database=TeccartDating");
            try
            {
                Conn.Open();
                Console.WriteLine("Connection successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return Conn;
        }
    }
}