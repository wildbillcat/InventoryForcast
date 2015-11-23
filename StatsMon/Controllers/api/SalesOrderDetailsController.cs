using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using StatsMon.Models;
using StatsMon.Models.Sales;
using StatsMon.Models.Sku;
using System.Data.Entity.Migrations;
using StatsCalc;

namespace StatsMon.Controllers.api
{
    public class SalesOrderDetailsController : ApiController
    {
        private StatusMonContext db = new StatusMonContext();

        // GET: api/SalesOrderDetails
        public IQueryable<SalesOrderDetail> GetSalesOrderDetails(int? Quantity, int? Page)
        {
            int Qty = Quantity ?? 100;
            int Pg = Page ?? 1;
            Pg--;
            IQueryable<SalesOrderDetail> ret = db.SalesOrderDetails.OrderBy(P => P.SalesOrderDetailID).Skip((Pg * Qty)).Take(Qty);
            return ret;
        }

        public async Task<int> GetSalesOrderDetailQuantity()
        {
            return await db.SalesOrderDetails.CountAsync();
        }

        // GET: api/SalesOrderDetails/5
        [ResponseType(typeof(SalesOrderDetail))]
        public async Task<IHttpActionResult> GetSalesOrderDetail(int id, int SalesOrderID)
        {
            SalesOrderDetail salesOrderDetail = await db.SalesOrderDetails.FindAsync(SalesOrderID, id);
            if (salesOrderDetail == null)
            {
                return NotFound();
            }

            return Ok(salesOrderDetail);
        }

        // PUT: api/SalesOrderDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSalesOrderDetail(int id, SalesOrderDetail salesOrderDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salesOrderDetail.SalesOrderID)
            {
                return BadRequest();
            }

            db.Entry(salesOrderDetail).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SalesOrderDetails
        [ResponseType(typeof(SalesOrderDetail))]
        public async Task<IHttpActionResult> PostSalesOrderDetail(SalesOrderDetail salesOrderDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SalesOrderDetails.Add(salesOrderDetail);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SalesOrderDetailExists(salesOrderDetail.SalesOrderID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            DateTime MostRecent = db.SalesOrderDetails.Where(p => p.ProductID == salesOrderDetail.ProductID).Max(o => o.SalesOrder.OrderDate).AddMonths(-1);
            double stdDev = StandardDeviation(salesOrderDetail.ProductID, MostRecent);
            int totalSales = TotalSales(salesOrderDetail.ProductID, MostRecent.AddMonths(-12), MostRecent);
            double[] forcastVals = Forcast(salesOrderDetail.ProductID, MostRecent);
            db.InventoryReports.AddOrUpdate(new InventoryReport(salesOrderDetail.ProductID, stdDev, forcastVals, totalSales));
            return CreatedAtRoute("DefaultApi", new { id = salesOrderDetail.SalesOrderID }, salesOrderDetail);
        }

        // DELETE: api/SalesOrderDetails/5
        [ResponseType(typeof(SalesOrderDetail))]
        public async Task<IHttpActionResult> DeleteSalesOrderDetail(int id, int SalesOrderID)
        {
            SalesOrderDetail salesOrderDetail = await db.SalesOrderDetails.FindAsync(SalesOrderID, id);
            if (salesOrderDetail == null)
            {
                return NotFound();
            }

            db.SalesOrderDetails.Remove(salesOrderDetail);
            await db.SaveChangesAsync();

            return Ok(salesOrderDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalesOrderDetailExists(int id)
        {
            return db.SalesOrderDetails.Count(e => e.SalesOrderID == id) > 0;
        }
        
        private int TotalSales(int SkuId, DateTime StartDate, DateTime EndDate)
        {
            StatusMonContext db = new StatusMonContext();
            int Sum = 0;
            if (db.SalesOrderDetails.Where(p =>
             p.ProductID == SkuId &&
             p.SalesOrder.OrderDate >= StartDate &&
             p.SalesOrder.OrderDate < EndDate).Count() > 0)
            {
                Sum = (db.SalesOrderDetails.Where(p =>
                p.ProductID == SkuId &&
                p.SalesOrder.OrderDate >= StartDate &&
                p.SalesOrder.OrderDate < EndDate).Sum(o => o.OrderQty));
            }
            db.Dispose();
            return Sum;
        }

        private List<int> MonthlySales(int SkuId, DateTime StartDate, DateTime EndDate)
        {
            List<int> SalesNumbers = new List<int>();
            for (DateTime ModifyDate = StartDate.Date; ModifyDate <= EndDate; ModifyDate = ModifyDate.AddMonths(1))
            {
                SalesNumbers.Add(TotalSales(SkuId, ModifyDate, ModifyDate.AddMonths(1)));
            }
            return SalesNumbers;
        }

        private List<double> MonthlySalesD(int SkuId, DateTime StartDate, DateTime EndDate)
        {
            List<int> SalesNumbers = MonthlySales(SkuId, StartDate, EndDate);
            return SalesNumbers.Select<int, double>(i => i).ToList();
        }

        private Double StandardDeviation(int SkuId, DateTime EndDate)
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
            else if (db.SalesOrderDetails.Where(o => o.SalesOrder.OrderDate.Date <= EndDate6).Count() > 0)
            {
                //6 Months
                SalesData = MonthlySalesD(SkuId, EndDate.AddMonths(-5), EndDate);
                xdata = new double[] { date - 5, date - 4, date - 3, date - 2, date - 1, date }; //Actual Values are nominal
            }
            else
            {
                //Not enough Data to forcast accurately
                db.Dispose();
                return new double[] { 0, 0, 0, 0 };
            }
            db.Dispose();
            ydata = SalesData.ToArray();

            return Linear.Forcast(date + 1, xdata, ydata);
        }
    }
}