using Dating.Models;
using System;
using System.Web.Mvc;
using MySqlConnector;

namespace Dating.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Profile()
        {
            // 1. Check if session variable is null
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            // Reload programming languages for the dropdown
            LoadProgrammingLanguages();

            string currentUsername = Session["Username"].ToString();
            Register user = new Register();

            // 2. Query MySQL to fetch the logged-in user's details
            string query = "SELECT * FROM Student WHERE Username = @Username";

            using (MySqlConnection conn = MySqlConnector())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", currentUsername);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user.FirstName = reader["Name"]?.ToString();
                            user.LastName = reader["FName"]?.ToString();

                            if (int.TryParse(reader["Age"]?.ToString(), out int age))
                            {
                                user.Age = age;
                            }

                            user.ProgrammingLanguage = reader["PLanguage"]?.ToString();
                            user.Username = reader["Username"]?.ToString();
                            user.Password = reader["Password"]?.ToString();
                        }
                    }
                }
            }

            return View(user);
        }

        // Logout method
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Profile(Register UpdatedUser)
        {
            // Check session validity
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            LoadProgrammingLanguages();

            string currentUsername = Session["Username"].ToString();

            // SQL Update statement matching your phpMyAdmin columns
            string query = "UPDATE Student SET Name = @Name, FName = @FName, Age = @Age, " +
                           "PLanguage = @PLanguage, Username = @NewUsername WHERE Username = @CurrentUsername";

            using (MySqlConnection conn = MySqlConnector())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", UpdatedUser.FirstName);
                    cmd.Parameters.AddWithValue("@FName", UpdatedUser.LastName);
                    cmd.Parameters.AddWithValue("@Age", UpdatedUser.Age);
                    cmd.Parameters.AddWithValue("@PLanguage", UpdatedUser.ProgrammingLanguage);
                    cmd.Parameters.AddWithValue("@NewUsername", UpdatedUser.Username);
                    cmd.Parameters.AddWithValue("@CurrentUsername", currentUsername);
                     
                    cmd.ExecuteNonQuery();
                }
            }

            // Update session variable in case they changed their username
            Session["Username"] = UpdatedUser.Username;
            ViewBag.UpdateMessage = "Profile updated successfully!";

            return View(UpdatedUser);
        }

        public void LoadProgrammingLanguages()
        {
            string[] programmingLanguages = new string[] { "C#", "Java", "Python", "JavaScript", "Ruby", "Go", "Rust", "Swift" };
            ViewBag.ProgrammingLanguages = programmingLanguages;
        }

        public MySqlConnection MySqlConnector()
        {
            MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=TeccartDating");
            conn.Open();
            return conn;
        }
    }
}