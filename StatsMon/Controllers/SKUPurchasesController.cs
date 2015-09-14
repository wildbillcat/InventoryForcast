using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StatsMon.Models;

namespace StatsMon.Controllers
{
    public class SKUPurchasesController : Controller
    {
        private StatusMonContext db = new StatusMonContext();

        // GET: SKUPurchases
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Forcast()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
