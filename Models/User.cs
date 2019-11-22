using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HighfieldTest.Models
{
    public class User
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public DateTime dob { get; set; }
        public string favouriteColour { get; set; }
    }
}