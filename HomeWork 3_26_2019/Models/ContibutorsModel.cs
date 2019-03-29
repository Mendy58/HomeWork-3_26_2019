using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaDBManager;

namespace HomeWork_3_26_2019.Models
{
    public class ContibutorsModel
    {
        public IEnumerable<Person> people { get; set; }
        public decimal TContrubutions { get; set; }
    }
}