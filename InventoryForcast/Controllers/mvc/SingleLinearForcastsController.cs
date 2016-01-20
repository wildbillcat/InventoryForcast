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
using InventoryForcast.Models.Calculations.Generators;

namespace InventoryForcast.Controllers.mvc
{
    [Authorize]
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
        public ActionResult Details(int? id)
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

        // GET: SingleLinearForcasts/Create
        public ActionResult Create()
        {
            return View();
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
            int sku = singleLinearForcast.SKU;
            db.SingleLinearForcasts.Remove(singleLinearForcast);
            db.SaveChanges();
            SingleLinearForcastGenerator.GenerateSingleLinearForcast(sku);
            return RedirectToAction("Details", new { id = sku });
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
