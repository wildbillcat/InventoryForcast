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

namespace InventoryForcast.Controllers.mvc
{
    public class MonthlyTotalsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MonthlyTotals
        public ActionResult Index(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(db.MonthlyTotals.OrderByDescending(s => s.Date).ToPagedList(pageNumber, pageSize));
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
