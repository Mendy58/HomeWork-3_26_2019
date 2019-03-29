using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaDBManager
{
    public class Contribution
    {
        public int id { get; set; }
        public int Simchaid { get; set; }
        public int Personid { get; set; }
        public decimal Amount { get; set; }
    }
}
