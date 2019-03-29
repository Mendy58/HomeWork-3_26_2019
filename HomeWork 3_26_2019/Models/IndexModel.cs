using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaDBManager;

namespace HomeWork_3_26_2019.Models
{
    public class IndexModel
    {
        public IEnumerable<Simcha> simchas { get; set; }
        public int Pplindb { get; set; }
    }
}