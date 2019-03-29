using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SimchaDBManager;
using HomeWork_3_26_2019.Models;

namespace HomeWork_3_26_2019.Controllers
{
    public class HomeController : Controller
    {
        SimchaManager mng = new SimchaManager(Properties.Settings.Default.SimchaConStr);
        public ActionResult Index()
        {
            IndexModel IModel = new IndexModel();
            IModel.simchas = mng.GetSimchas();
            IModel.Pplindb = mng.GetPeopleCount();
            return View(IModel);
        }

        public ActionResult Contributors()
        {
            ContibutorsModel CModel = new ContibutorsModel(); 
            CModel.people = mng.GetPeople();
            return View(CModel);
        }

        public ActionResult Contributions(int id)
        {
            IEnumerable<Transaction> transactions = mng.GetTransactionsbyidSorted(id);
            return View(transactions);
        }

        public ActionResult AddContributionsView(int id)
        {
            AddContribModel ACModel = new AddContribModel();
            ACModel.people = mng.GetPeople();
            ACModel.Simchaid = id;
            return View(ACModel);
        }

        [HttpPost]
        public ActionResult AddSimcha(Simcha s)
        {
            mng.AddSimcha(s);
            return Redirect("/");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}