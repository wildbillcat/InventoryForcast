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
    }
}