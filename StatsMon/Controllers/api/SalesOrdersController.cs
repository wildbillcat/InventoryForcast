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

namespace StatsMon.Controllers.api
{
    public class SalesOrdersController : ApiController
    {
        private StatusMonContext db = new StatusMonContext();

        // GET: api/SalesOrders
        public IQueryable<SalesOrder> GetSalesOrders(int? Quantity, int? Page)
        {
            int Qty = Quantity ?? 100;
            int Pg = Page ?? 1;
            Pg--;
            IQueryable<SalesOrder> ret = db.SalesOrders.OrderBy(P=>P.SalesOrderID).Skip((Pg * Qty)).Take(Qty);
            return ret;
        }

        public async Task<int> GetSalesOrderQuantity()
        {
            return await db.SalesOrders.CountAsync();
        }

        // GET: api/SalesOrders/5
        [ResponseType(typeof(SalesOrder))]
        public async Task<IHttpActionResult> GetSalesOrder(long id)
        {
            SalesOrder SalesOrder = await db.SalesOrders.FindAsync(id);
            if (SalesOrder == null)
            {
                return NotFound();
            }

            return Ok(SalesOrder);
        }

        // PUT: api/SalesOrders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSalesOrder(long id, SalesOrder SalesOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != SalesOrder.SalesOrderID)
            {
                return BadRequest();
            }

            db.Entry(SalesOrder).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderExists(id))
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

        // POST: api/SalesOrders
        [ResponseType(typeof(SalesOrder))]
        public async Task<IHttpActionResult> PostSalesOrder(SalesOrder salesOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SalesOrders.Add(salesOrder);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SalesOrderExists(salesOrder.SalesOrderID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = salesOrder.SalesOrderID }, salesOrder);
        }

        // DELETE: api/SalesOrders/5
        [ResponseType(typeof(SalesOrder))]
        public async Task<IHttpActionResult> DeleteSalesOrder(long id)
        {
            SalesOrder SalesOrder = await db.SalesOrders.FindAsync(id);
            if (SalesOrder == null)
            {
                return NotFound();
            }

            db.SalesOrders.Remove(SalesOrder);
            await db.SaveChangesAsync();

            return Ok(SalesOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalesOrderExists(long id)
        {
            return db.SalesOrders.Count(e => e.SalesOrderID == id) > 0;
        }
    }
}