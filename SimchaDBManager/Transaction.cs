using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaDBManager
{
    public class Transaction
    {
        public int id { get; set; }
        public string Action {get; set;}
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
