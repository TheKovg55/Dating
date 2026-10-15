using Dating.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MySqlConnector;

namespace Dating.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search with multi-criteria filters querying MySQL
        public ActionResult Search(string username = null, int? minAge = null, int? maxAge = null, string programmingLanguage = null)
        {
            // Ensure that the user is logged in
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            LoadProgrammingLanguages();

            string currentUsername = Session["Username"].ToString();
            List<Register> userList = new List<Register>();

            // Build base SQL query, excluding the currently logged-in user
            string query = "SELECT * FROM Student WHERE Username != @CurrentUsername";

            // Dynamically append filters if provided by the user
            if (!string.IsNullOrWhiteSpace(username))
            {
                query += " AND (Name LIKE @Username OR FName LIKE @Username OR Username LIKE @Username)";
            }
            if (minAge.HasValue)
            {
                query += " AND Age >= @MinAge";
            }
            if (maxAge.HasValue)
            {
                query += " AND Age <= @MaxAge";
            }
            if (!string.IsNullOrWhiteSpace(programmingLanguage))
            {
                query += " AND PLanguage = @PLanguage";
            }

            using (MySqlConnection conn = MySqlConnector())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CurrentUsername", currentUsername);

                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        cmd.Parameters.AddWithValue("@Username", "%" + username + "%");
                    }
                    if (minAge.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@MinAge", minAge.Value);
                    }
                    if (maxAge.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@MaxAge", maxAge.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(programmingLanguage))
                    {
                        cmd.Parameters.AddWithValue("@PLanguage", programmingLanguage);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Register user = new Register();
                            user.FirstName = reader["Name"]?.ToString();
                            user.LastName = reader["FName"]?.ToString();

                            if (int.TryParse(reader["Age"]?.ToString(), out int age))
                            {
                                user.Age = age;
                            }

                            user.ProgrammingLanguage = reader["PLanguage"]?.ToString();
                            user.Username = reader["Username"]?.ToString();

                            userList.Add(user);
                        }
                    }
                }
            }

            return View(userList);
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