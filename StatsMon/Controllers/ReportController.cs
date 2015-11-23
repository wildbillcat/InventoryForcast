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
using System.Data.Entity.Migrations;

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

        public ActionResult Generate()
        {
            StatusMonContext db = new StatusMonContext();
            DateTime MostRecent = db.SalesOrders.Max(s => s.OrderDate).Date;
            List<int> Skus = db.SalesOrderDetails.GroupBy(o => o.ProductID).ToList().Select(p => p.Key).ToList();
            //This Bag will be used to store the Reports
            ConcurrentBag<InventoryReport> InventoryReports = new ConcurrentBag<InventoryReport>();

            //Parallel.ForEach(Skus, (Sku) =>
            //{
            foreach (int Sku in Skus)
            {
                double stdDev = StandardDeviation(Sku, MostRecent);
                int totalSales = TotalSales(Sku, MostRecent.AddMonths(-12), MostRecent);
                double[] forcastVals = Forcast(Sku, MostRecent);
                db.InventoryReports.AddOrUpdate(new InventoryReport(Sku, stdDev, forcastVals, totalSales));
            }
            //});
            return RedirectToAction("Index");
        }

        private int TotalSales(int SkuId, DateTime StartDate, DateTime EndDate)
        {
            StatusMonContext db = new StatusMonContext();
            int Sum = 0;
            if (db.SalesOrderDetails.Where(p =>
             p.ProductID == SkuId &&
             p.SalesOrder.OrderDate <= StartDate &&
             p.SalesOrder.OrderDate <= EndDate).Count() > 0)
            {
                Sum = (db.SalesOrderDetails.Where(p =>
                p.ProductID == SkuId &&
                p.SalesOrder.OrderDate <= StartDate &&
                p.SalesOrder.OrderDate <= EndDate).Sum(o => o.OrderQty));
            }
            db.Dispose();
            return Sum;
        }

        private List<int> MonthlySales(int SkuId, DateTime StartDate, DateTime EndDate)
        {
            List<int> SalesNumbers = new List<int>();
            for (DateTime ModifyDate = StartDate.Date; ModifyDate <= EndDate; ModifyDate = ModifyDate.AddMonths(1))
            {
                SalesNumbers.Add(TotalSales(SkuId, ModifyDate, ModifyDate));
            }
            return SalesNumbers;
        }

        private List<double> MonthlySalesD(int SkuId, DateTime StartDate, DateTime EndDate)
        {
            List<int> SalesNumbers = MonthlySales(SkuId, StartDate, EndDate);
            return SalesNumbers.Select<int, double>(i => i).ToList();
        }

        private Double StandardDeviation (int SkuId, DateTime EndDate)
        {
            //This Loop Builds the array of Sales Data for the 6 Months
            List<double> SalesNumbers = MonthlySalesD(SkuId, EndDate.Date.AddMonths(-6), EndDate.Date);
            double average = SalesNumbers.Average();
            double sumOfSquaresOfDifferences = SalesNumbers.Select(val => (val - average) * (val - average)).Sum();
            double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / SalesNumbers.Count());
            return standardDeviation;
        }

        /// <summary>
        /// Should forcast using 12 months data, falling back to 6 month if unavailable
        /// //ForcastSlope = _ForcastVals[0];
            //ForcastIntercept = _ForcastVals[1];
            //ForcastValue = _ForcastVals[2];
            //ForcastRangeUpperBount = _ForcastVals[3];
        /// </summary>
        /// <param name="SKUID"></param>
        private double[] Forcast(int SkuId, DateTime EndDate)
        {
            StatusMonContext db = new StatusMonContext();
            List<double> SalesData;
            double[] xdata;
            double[] ydata;
            DateTime EndDate12 = EndDate.AddMonths(-12);
            DateTime EndDate6 = EndDate.AddMonths(-6);
            int date = (EndDate.Date.Year * 12 + EndDate.Date.Month);
            //Test if 12 Month of Data Exists
            if (db.SalesOrderDetails.Where(o => o.SalesOrder.OrderDate <= EndDate12).Count() > 0)
            {
                SalesData = MonthlySalesD(SkuId, EndDate.AddMonths(-11), EndDate);
                xdata = new double[] { date - 11, date - 10, date - 9, date - 8, date - 7, date - 6, date - 5, date - 4, date - 3, date - 2, date - 1, date };
                //12 Months!
            }
            else if (db.SalesOrderDetails.Where(o => o.SalesOrder.OrderDate.Date <= EndDate6).Count() > 0) {
                //6 Months
                SalesData = MonthlySalesD(SkuId, EndDate.AddMonths(-5), EndDate);
                xdata = new double[] { date - 5, date - 4, date - 3, date - 2, date - 1, date }; //Actual Values are nominal
            }
            else
            {
                //Not enough Data to forcast accurately
                db.Dispose();
                return new double[] { 0, 0, 0, 0};
            }
            db.Dispose();
            ydata = SalesData.ToArray();

            return Linear.Forcast(date + 1, xdata, ydata);
        }        
    }
}
