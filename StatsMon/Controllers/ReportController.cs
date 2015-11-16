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
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace StatsMon.Controllers
{
    public class ReportController : Controller
    {

        // GET: SalesOrderDetails
        public ActionResult Index()
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
                InventoryReports.Add(new InventoryReport(Sku, stdDev, forcastVals, totalSales));
            }
             //});

            return View(InventoryReports);
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
            for (DateTime ModifyDate = StartDate.Date; ModifyDate <= EndDate.Date; ModifyDate = ModifyDate.AddMonths(1))
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
            //Test if 12 Month of Data Exists
            if (db.SalesOrderDetails.Where(o => o.SalesOrder.OrderDate <= EndDate12).Count() > 0)
            {
                SalesData = MonthlySalesD(SkuId, EndDate.AddMonths(-11), EndDate);
                xdata = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                //12 Months!
            }
            else if (db.SalesOrderDetails.Where(o => o.SalesOrder.OrderDate.Date <= EndDate6).Count() > 0) {
                //6 Months
                SalesData = MonthlySalesD(SkuId, EndDate.AddMonths(-5), EndDate);
                xdata = new double[] { 1, 2, 3, 4, 5, 6 }; //Actual Values are nominal
            }
            else
            {
                //Not enough Data to forcast accurately
                db.Dispose();
                return new double[] { 0, 0, 0, 0};
            }
            db.Dispose();
            ydata = SalesData.ToArray();
            
            Tuple<double, double> p = Fit.Line(xdata, ydata);
            double ForcastIntercept = p.Item1; // intercept
            double ForcastSlope = p.Item2; // slope
            double ForcastRangeUpperBound = xdata.Last();
            double PredictedValue = ((ForcastSlope * (ForcastRangeUpperBound + 1)) + ForcastIntercept); //Next Predicted Value (ie month 7 or 13)
            return new double[] { ForcastIntercept, ForcastSlope, PredictedValue, ForcastRangeUpperBound };
        }        
    }
}
