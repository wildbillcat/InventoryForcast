using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StatsMon.Models;
using StatsMon.Models.Sales;
using StatsMon.Models.Sku;
using PagedList.Mvc;
using PagedList;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using StatsCalc;

namespace StatsMon.Controllers
{
    public class ReportController : Controller
    {

        // GET: SalesOrderDetails
        public ActionResult Index()
        {
            StatusMonContext db = new StatusMonContext();
            
            

            return View(db.InventoryReports.ToList());
        }

        //public ActionResult SalesOrders2(int? page)
        //{
        //    int pageSize = 20;
        //    int pageNumber = (page ?? 1);
        //    return View(db.SalesOrders.OrderBy(s => s.SalesOrderID).ToPagedList(pageNumber, pageSize));
        //}
    }
}
