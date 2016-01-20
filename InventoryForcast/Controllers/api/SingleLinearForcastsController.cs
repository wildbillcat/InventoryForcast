using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using InventoryForcast.Models;
using InventoryForcast.Models.Calculations;
using InventoryForcast.Models.Calculations.Generators;

namespace InventoryForcast.Controllers.api
{
    [Authorize]
    public class SingleLinearForcastsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SingleLinearForcasts
        public IQueryable<SingleLinearForcast> GetSingleLinearForcasts()
        {
            return db.SingleLinearForcasts.OrderByDescending(P => P.Month_Id).Take(100);
        }

        // GET: api/SingleLinearForcasts/5
        [ResponseType(typeof(SingleLinearForcast))]
        public IHttpActionResult GetSingleLinearForcast(int id)
        {
            SingleLinearForcast singleLinearForcast = db.SingleLinearForcasts.Find(id);
            
            if (singleLinearForcast == null)
            {
                SingleLinearForcastGenerator.GenerateSingleLinearForcast(id);
            }

            singleLinearForcast = db.SingleLinearForcasts.Find(id);

            if (singleLinearForcast == null)
            {
                return NotFound();
            }

            return Ok(singleLinearForcast);
        }

        // PUT: api/SingleLinearForcasts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSingleLinearForcast(int id, SingleLinearForcast singleLinearForcast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != singleLinearForcast.SKU)
            {
                return BadRequest();
            }

            db.Entry(singleLinearForcast).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SingleLinearForcastExists(id))
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

        // POST: api/SingleLinearForcasts
        [ResponseType(typeof(SingleLinearForcast))]
        public IHttpActionResult PostSingleLinearForcast(SingleLinearForcast singleLinearForcast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SingleLinearForcasts.Add(singleLinearForcast);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SingleLinearForcastExists(singleLinearForcast.SKU))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = singleLinearForcast.SKU }, singleLinearForcast);
        }

        // DELETE: api/SingleLinearForcasts/5
        [ResponseType(typeof(SingleLinearForcast))]
        public IHttpActionResult DeleteSingleLinearForcast(int id)
        {
            SingleLinearForcast singleLinearForcast = db.SingleLinearForcasts.Find(id);
            if (singleLinearForcast == null)
            {
                return NotFound();
            }

            db.SingleLinearForcasts.Remove(singleLinearForcast);
            db.SaveChanges();

            return Ok(singleLinearForcast);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SingleLinearForcastExists(int id)
        {
            return db.SingleLinearForcasts.Count(e => e.SKU == id) > 0;
        }
    }
}