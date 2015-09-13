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

        // GET: SKUPurchases/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKUPurchase sKUPurchase = db.SKUPurchases.Find(id);
            if (sKUPurchase == null)
            {
                return HttpNotFound();
            }
            return View(sKUPurchase);
        }

        // GET: SKUPurchases/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SKUPurchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PurchaseID,Quantity")] SKUPurchase sKUPurchase)
        {
            if (ModelState.IsValid)
            {
                db.SKUPurchases.Add(sKUPurchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sKUPurchase);
        }

        // GET: SKUPurchases/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKUPurchase sKUPurchase = db.SKUPurchases.Find(id);
            if (sKUPurchase == null)
            {
                return HttpNotFound();
            }
            return View(sKUPurchase);
        }

        // POST: SKUPurchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PurchaseID,Quantity")] SKUPurchase sKUPurchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sKUPurchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sKUPurchase);
        }

        // GET: SKUPurchases/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKUPurchase sKUPurchase = db.SKUPurchases.Find(id);
            if (sKUPurchase == null)
            {
                return HttpNotFound();
            }
            return View(sKUPurchase);
        }

        // POST: SKUPurchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            SKUPurchase sKUPurchase = db.SKUPurchases.Find(id);
            db.SKUPurchases.Remove(sKUPurchase);
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
