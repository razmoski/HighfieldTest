using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HighfieldTest.Models
{
    public class AgeAndColours
    {
        public List<User>      users { get; set; }
        public List<Age>       ages { get; set; }
        public List<TopColour> topColours { get; set; }        
    }
}