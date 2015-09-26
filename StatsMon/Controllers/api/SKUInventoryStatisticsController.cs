//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Description;
//using StatsMon.Models;

//namespace StatsMon.Controllers.api
//{
//    public class SKUInventoryStatisticsController : ApiController
//    {
//        private StatusMonContext db = new StatusMonContext();

//        // GET: api/SKUInventoryStatistics
//        public IQueryable<SKUInventoryStatistic> GetSKUInventoryStatistics()
//        {
//            return db.SKUInventoryStatistics;
//        }

//        // GET: api/SKUInventoryStatistics/5
//        [ResponseType(typeof(SKUInventoryStatistic))]
//        public async Task<IHttpActionResult> GetSKUInventoryStatistic(long id)
//        {
//            SKUInventoryStatistic sKUInventoryStatistic = await db.SKUInventoryStatistics.FindAsync(id);
//            if (sKUInventoryStatistic == null)
//            {
//                return NotFound();
//            }

//            return Ok(sKUInventoryStatistic);
//        }

//        // PUT: api/SKUInventoryStatistics/5
//        [ResponseType(typeof(void))]
//        public async Task<IHttpActionResult> PutSKUInventoryStatistic(long id, SKUInventoryStatistic sKUInventoryStatistic)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            if (id != sKUInventoryStatistic.Id)
//            {
//                return BadRequest();
//            }

//            db.Entry(sKUInventoryStatistic).State = EntityState.Modified;

//            try
//            {
//                await db.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!SKUInventoryStatisticExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return StatusCode(HttpStatusCode.NoContent);
//        }

//        // POST: api/SKUInventoryStatistics
//        [ResponseType(typeof(SKUInventoryStatistic))]
//        public async Task<IHttpActionResult> PostSKUInventoryStatistic(SKUInventoryStatistic sKUInventoryStatistic)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            db.SKUInventoryStatistics.Add(sKUInventoryStatistic);
//            await db.SaveChangesAsync();

//            return CreatedAtRoute("DefaultApi", new { id = sKUInventoryStatistic.Id }, sKUInventoryStatistic);
//        }

//        // DELETE: api/SKUInventoryStatistics/5
//        [ResponseType(typeof(SKUInventoryStatistic))]
//        public async Task<IHttpActionResult> DeleteSKUInventoryStatistic(long id)
//        {
//            SKUInventoryStatistic sKUInventoryStatistic = await db.SKUInventoryStatistics.FindAsync(id);
//            if (sKUInventoryStatistic == null)
//            {
//                return NotFound();
//            }

//            db.SKUInventoryStatistics.Remove(sKUInventoryStatistic);
//            await db.SaveChangesAsync();

//            return Ok(sKUInventoryStatistic);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private bool SKUInventoryStatisticExists(long id)
//        {
//            return db.SKUInventoryStatistics.Count(e => e.Id == id) > 0;
//        }
//    }
//}