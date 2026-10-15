using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Dating.Models;
using MySqlConnector;
namespace Dating.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {

            //Loading the programming languages in the view when the page is loaded
            LoadProgrammingLanguages();

            //Creating a list to store the students in:
            List<Register> Students = new List<Register>();
            
            
            //Creating a connection to the database: 
            MySqlConnection Conn = MySqlConnector();


            //Apply the command
            MySqlCommand SelectAll = new MySqlCommand("Select * from Student", Conn);

            //Executing the command

            MySqlDataReader reader = SelectAll.ExecuteReader();

            //A quick loop in order to manually read everything
            while (reader.Read())
            {
                //Create an object
                Register Student = new Register();

                //Affecting values to the attributes:
                Student.FirstName = reader["Name"].ToString();
                Student.FirstName = reader["FName"].ToString();
                Student.FirstName = reader["Age"].ToString();
                Student.FirstName = reader["PLanguage"].ToString();
                Student.FirstName = reader["Username"].ToString();
                Student.FirstName = reader["Password"].ToString();

                //Adding to List
                Students.Add(Student);

            }
            //Added this to add a stop point in order to check if connection worked or not
            int a = Students.Count();

            //Closing connection after insert is done
            Conn.Close();

            return View();

            //putting the programming languages in a string array to be used in the view
            /*
            string[] programmingLanguages = new string[] { "C#", "Java", "Python", "JavaScript", "Ruby", "Go", "Rust", "Swift" };
            ViewBag.ProgrammingLanguages = programmingLanguages;
            return View();
            */



        }


        [HttpPost]
        public ActionResult Index(Register user)
        {
            //Loading the programming languages in the view after the form is submitted
            LoadProgrammingLanguages();

            try
            {
                //Executing the saving method to save into the database directly
                SaveData(user);

                //Showing Alert Message:
                ViewBag.AlertMesssage = "Registration is successful!";

                //New method to clear the form
                ModelState.Clear();

            }
            catch(Exception ex)
            {
                //if it fails do this:
                ViewBag.AlertMessage = "Registration failed " +ex.Message ;

            }

           
            return View(user);
        }

        public void LoadProgrammingLanguages()
        {
            string[] programmingLanguages = new string[] { "C#", "Java", "Python", "JavaScript", "Ruby", "Go", "Rust", "Swift" };
            ViewBag.ProgrammingLanguages = programmingLanguages;
        }

        public void SaveData(Register user)
        {
            //Sql Statement to insert into the database:
            string query = "Insert into Student (Name, FName, Age, PLanguage, Username, Password) " + "VALUES (@Name, @FName, @Age, @PLanguage, @Username, @Password)";

            //Creating the object for the connection, using the method created below
            MySqlConnection conn = MySqlConnector();

            //Affecting the query to the mysql command
            MySqlCommand cmd = new MySqlCommand(query, conn);

            //Time for the parameters
            cmd.Parameters.AddWithValue("@Name", user.FirstName);
            cmd.Parameters.AddWithValue("@FName", user.LastName);
            cmd.Parameters.AddWithValue("@Age", user.Age);
            cmd.Parameters.AddWithValue("@PLanguage", user.ProgrammingLanguage);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Password", user.Password);

            //Executing the query!
            cmd.ExecuteNonQuery();

            conn.Close();



        }

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