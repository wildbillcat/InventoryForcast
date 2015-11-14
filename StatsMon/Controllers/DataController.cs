using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatsMon.Models;
using StatsMon.Models.Sales;
using PagedList.Mvc;
using PagedList;

namespace StatsMon.Controllers
{
    public class DataController : Controller
    {
        private StatusMonContext db = new StatusMonContext();

        // GET: Data
        public ActionResult Index()
        {
            return View();
        }


        //Angular Methods
        // GET: SalesOrder SPA
        public ActionResult SalesOrders()
        {
            return View();
        }

        // GET: SalesOrderDetail SPA
        public ActionResult SalesOrderDetails()
        {
            return View();
        }
        //End Angular Methods

        public ActionResult SalesOrders2(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(db.SalesOrders.OrderBy(s => s.SalesOrderID).ToPagedList(pageNumber, pageSize));
        }

        // GET: SalesOrderDetail SPA
        public ActionResult SalesOrderDetails2(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(db.SalesOrderDetails.OrderBy(s => s.SalesOrderDetailID).ToPagedList(pageNumber, pageSize));
        }
    }
}