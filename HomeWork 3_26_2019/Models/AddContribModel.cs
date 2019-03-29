using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaDBManager;

namespace HomeWork_3_26_2019.Models
{
    public class AddContribModel
    {
        public IEnumerable<Person> people { get; set; }
        public int Simchaid { get; set; }
    }
}