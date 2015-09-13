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

namespace StatsMon.Controllers.api
{
    public class SKUPurchasesController : ApiController
    {
        private StatusMonContext db = new StatusMonContext();

        // GET: api/SKUPurchases
        public IQueryable<SKUPurchase> GetSKUPurchases(int? Quantity, int? Page)
        {
            int Qty = Quantity ?? 100;
            int Pg = Page ?? 1;
            Pg--;
            IQueryable<SKUPurchase> ret = db.SKUPurchases.OrderBy(P=>P.Id).Skip((Pg * Qty)).Take(Qty);
            return ret;
        }

        public async Task<int> GetSKUPurchaseQuantity()
        {
            return await db.SKUPurchases.CountAsync();
        }

        // GET: api/SKUPurchases/5
        [ResponseType(typeof(SKUPurchase))]
        public async Task<IHttpActionResult> GetSKUPurchase(long id, long PurchaseID)
        {
            SKUPurchase sKUPurchase = await db.SKUPurchases.FindAsync(id, PurchaseID);
            if (sKUPurchase == null)
            {
                return NotFound();
            }

            return Ok(sKUPurchase);
        }

        // PUT: api/SKUPurchases/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSKUPurchase(long id, long PurchaseID, SKUPurchase sKUPurchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sKUPurchase.Id || PurchaseID != sKUPurchase.Id)
            {
                return BadRequest();
            }

            db.Entry(sKUPurchase).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SKUPurchaseExists(id, PurchaseID))
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

        // POST: api/SKUPurchases
        [ResponseType(typeof(SKUPurchase))]
        public async Task<IHttpActionResult> PostSKUPurchase(SKUPurchase sKUPurchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SKUPurchases.Add(sKUPurchase);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SKUPurchaseExists(sKUPurchase.Id, sKUPurchase.PurchaseID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = sKUPurchase.Id }, sKUPurchase);
        }

        // DELETE: api/SKUPurchases/5
        [ResponseType(typeof(SKUPurchase))]
        public async Task<IHttpActionResult> DeleteSKUPurchase(long id, long PurchaseID)
        {
            SKUPurchase sKUPurchase = await db.SKUPurchases.FindAsync(id, PurchaseID);
            if (sKUPurchase == null)
            {
                return NotFound();
            }

            db.SKUPurchases.Remove(sKUPurchase);
            await db.SaveChangesAsync();

            return Ok(sKUPurchase);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SKUPurchaseExists(long id, long PurchaseID)
        {
            return db.SKUPurchases.Count(e => e.Id == id && e.PurchaseID == PurchaseID) > 0;
        }
    }
}