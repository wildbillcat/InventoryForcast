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
    public class SingleLinearForcastsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SingleLinearForcasts
        public ActionResult Index(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(db.SingleLinearForcasts.OrderByDescending(s => s.Date).ThenBy(H => H.SKU).ToPagedList(pageNumber, pageSize));
        }

        // GET: SingleLinearForcasts/Details/5
        public ActionResult Details(int? id, int? Month_Id)
        {
            if (id == null || Month_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SingleLinearForcast singleLinearForcast = db.SingleLinearForcasts.Find(id, Month_Id);
            if (singleLinearForcast == null)
            {
                return HttpNotFound();
            }
            return View(singleLinearForcast);
        }

        // GET: SingleLinearForcasts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SingleLinearForcasts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SKU,Month_Id,Date,Quantity_Forcast,Absolute_Quantity_Forcast,Slope,Intercept,JSON_MonthlyTotals,Sample_Size,SkuClass,Valid")] SingleLinearForcast singleLinearForcast)
        {
            if (ModelState.IsValid)
            {
                db.SingleLinearForcasts.Add(singleLinearForcast);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(singleLinearForcast);
        }

        // GET: SingleLinearForcasts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SingleLinearForcast singleLinearForcast = db.SingleLinearForcasts.Find(id);
            if (singleLinearForcast == null)
            {
                return HttpNotFound();
            }
            return View(singleLinearForcast);
        }

        // POST: SingleLinearForcasts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SKU,Month_Id,Date,Quantity_Forcast,Absolute_Quantity_Forcast,Slope,Intercept,JSON_MonthlyTotals,Sample_Size,SkuClass,Valid")] SingleLinearForcast singleLinearForcast)
        {
            if (ModelState.IsValid)
            {
                db.Entry(singleLinearForcast).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(singleLinearForcast);
        }

        // GET: SingleLinearForcasts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SingleLinearForcast singleLinearForcast = db.SingleLinearForcasts.Find(id);
            if (singleLinearForcast == null)
            {
                return HttpNotFound();
            }
            return View(singleLinearForcast);
        }

        // POST: SingleLinearForcasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SingleLinearForcast singleLinearForcast = db.SingleLinearForcasts.Find(id);
            db.SingleLinearForcasts.Remove(singleLinearForcast);
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
