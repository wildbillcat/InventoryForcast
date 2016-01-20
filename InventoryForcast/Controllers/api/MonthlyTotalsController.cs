using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using InventoryForcast.Models;
using InventoryForcast.Models.Calculations;

namespace InventoryForcast.Controllers.api
{
    [Authorize]
    public class MonthlyTotalsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/MonthlyTotals
        public IQueryable<MonthlyTotal> GetMonthlyTotals()
        {
            return db.MonthlyTotals;
        }

        // GET: api/MonthlyTotals/5
        [ResponseType(typeof(MonthlyTotal))]
        public IHttpActionResult GetMonthlyTotal(int id)
        {
            MonthlyTotal monthlyTotal = db.MonthlyTotals.Find(id);
            if (monthlyTotal == null)
            {
                return NotFound();
            }

            return Ok(monthlyTotal);
        }

        // PUT: api/MonthlyTotals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMonthlyTotal(int id, MonthlyTotal monthlyTotal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != monthlyTotal.SKU)
            {
                return BadRequest();
            }

            db.Entry(monthlyTotal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonthlyTotalExists(id))
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

        // POST: api/MonthlyTotals
        [ResponseType(typeof(MonthlyTotal))]
        public IHttpActionResult PostMonthlyTotal(MonthlyTotal monthlyTotal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Ensure that absolute value has been calculated
            monthlyTotal.Absolute_Quantity_Sold = MonthlyTotal.RemoveSeasonality(monthlyTotal.Quantity_Sold, monthlyTotal.Date.Month);
            db.MonthlyTotals.Add(monthlyTotal);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MonthlyTotalExists(monthlyTotal.SKU))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = monthlyTotal.SKU }, monthlyTotal);
        }

        // DELETE: api/MonthlyTotals/5
        [ResponseType(typeof(MonthlyTotal))]
        public IHttpActionResult DeleteMonthlyTotal(int id)
        {
            MonthlyTotal monthlyTotal = db.MonthlyTotals.Find(id);
            if (monthlyTotal == null)
            {
                return NotFound();
            }

            db.MonthlyTotals.Remove(monthlyTotal);
            db.SaveChanges();

            return Ok(monthlyTotal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MonthlyTotalExists(int id)
        {
            return db.MonthlyTotals.Count(e => e.SKU == id) > 0;
        }
    }
}