using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryForcast.Models;
using InventoryForcast.Models.Calculations;
using PagedList.Mvc;
using PagedList;
using CsvHelper;

namespace InventoryForcast.Controllers.mvc
{
    [Authorize]
    public class MonthlyTotalsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MonthlyTotals
        public ActionResult Index(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(db.MonthlyTotals.OrderByDescending(s => s.Date).ThenBy(H => H.SKU).ToPagedList(pageNumber, pageSize));
        }

        // GET: MonthlyTotals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyTotal monthlyTotal = db.MonthlyTotals.Find(id);
            if (monthlyTotal == null)
            {
                return HttpNotFound();
            }
            return View(monthlyTotal);
        }

        // GET: MonthlyTotals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MonthlyTotals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SKU,Month_Id,Quantity_Sold,Absolute_Quantity_Sold,Date")] MonthlyTotal monthlyTotal)
        {
            if (ModelState.IsValid)
            {
                db.MonthlyTotals.Add(monthlyTotal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(monthlyTotal);
        }

        // GET: MonthlyTotals/Create
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportPreview(HttpPostedFileBase CsvFile)
        {
            List<MonthlyTotal> Totals = new List<MonthlyTotal>();
            string saveAsDirectory = string.Concat(AppDomain.CurrentDomain.GetData("DataDirectory"), "\\csvimport");
            if (CsvFile.FileName.ToLower().Split('.').Last().Equals("csv"))
            {
                //Validate SaveAs Directory is present
                if (!System.IO.Directory.Exists(saveAsDirectory))
                {
                    System.IO.Directory.CreateDirectory(saveAsDirectory);
                }
                CsvFile.SaveAs(string.Concat(saveAsDirectory, "\\", CsvFile.FileName));
                ViewData["CsvFile"] = CsvFile.FileName;
                DateTime SalesDate = DateTime.Now; //Placeholder
                //int MonthID = 12 * SalesDate.Month * SalesDate.Year;
                using (var csv = new CsvReader(System.IO.File.OpenText(string.Concat(saveAsDirectory, "\\", CsvFile.FileName))))
                {
                    int i = 0;
                    while (csv.Read())
                    {
                        if (i > 19)
                        {
                            break;
                        }
                        Totals.Add(new MonthlyTotal() {
                            SKU = csv.GetField<int>(0),
                            Quantity_Sold = csv.GetField<int>(1),
                            Absolute_Quantity_Sold = 0, //PlaceHolder
                            Date = SalesDate, //PlaceHolder
                            Month_Id = 0 //Placeholder
                        }
                            );
                        i++;
                    }
                }
            }
            return View(Totals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportCommit(string CsvFile, DateTime CsvDate)
        {
            string saveAsDirectory = string.Concat(AppDomain.CurrentDomain.GetData("DataDirectory"), "\\csvimport");
            if (System.IO.File.Exists(string.Concat(saveAsDirectory, "\\", CsvFile)))
            {
                DateTime SalesDate = CsvDate.Date;
                SalesDate.AddDays(-1 * (SalesDate.Day - 1));//Ensures Day of the first of the month
                int MonthID = 12 * SalesDate.Month * SalesDate.Year;
                using (var csv = new CsvReader(System.IO.File.OpenText(string.Concat(saveAsDirectory, "\\", CsvFile))))
                {
                    while (csv.Read())
                    {
                        ApplicationDbContext ctx = new ApplicationDbContext();
                        int _qty_sold = csv.GetField<int>(1);
                        ctx.MonthlyTotals.Add(new MonthlyTotal()
                        {
                            SKU = csv.GetField<int>(0),
                            Quantity_Sold = _qty_sold,
                            Absolute_Quantity_Sold = MonthlyTotal.RemoveSeasonality(_qty_sold, SalesDate.Month),
                            Date = SalesDate, 
                            Month_Id = MonthID
                        }
                            );
                        ctx.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: MonthlyTotals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyTotal monthlyTotal = db.MonthlyTotals.Find(id);
            if (monthlyTotal == null)
            {
                return HttpNotFound();
            }
            return View(monthlyTotal);
        }

        // POST: MonthlyTotals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SKU,Month_Id,Quantity_Sold,Absolute_Quantity_Sold,Date")] MonthlyTotal monthlyTotal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monthlyTotal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(monthlyTotal);
        }

        // GET: MonthlyTotals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyTotal monthlyTotal = db.MonthlyTotals.Find(id);
            if (monthlyTotal == null)
            {
                return HttpNotFound();
            }
            return View(monthlyTotal);
        }

        // POST: MonthlyTotals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MonthlyTotal monthlyTotal = db.MonthlyTotals.Find(id);
            db.MonthlyTotals.Remove(monthlyTotal);
            db.SaveChanges();
            return RedirectToAction("Index");
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
