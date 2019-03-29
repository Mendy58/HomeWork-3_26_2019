using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaDBManager
{
    public class Person
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cell { get; set; }
        public bool dContributor { get; set; }
        public int Age { get; set; }
        public decimal Balance { get; set; }
    }
}
