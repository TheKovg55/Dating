using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dating.Models
{
    public class Register
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string ProgrammingLanguage { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}