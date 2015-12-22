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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StatsCalc;

namespace InventoryForcast.Controllers.api
{
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
        public IHttpActionResult GetSingleLinearForcast(int id, int Month_Id)
        {
            SingleLinearForcast singleLinearForcast = db.SingleLinearForcasts.Find(id, Month_Id);
            
            if (singleLinearForcast == null)
            {
                GenerateSingleLinearForcast(id, Month_Id);
            }

            singleLinearForcast = db.SingleLinearForcasts.Find(id, Month_Id);

            if (singleLinearForcast == null)
            {
                return NotFound();
            }

            return Ok(singleLinearForcast);
        }

        private void GenerateSingleLinearForcast(int id, int Month_Id) {
            int month = Month_Id % 12;
            int year = (Month_Id - month) / 12;
            DateTime ForcastDate = new DateTime(year,month,1);
            List<MonthlyTotal> Totals = db.MonthlyTotals.Where(P => P.SKU == id && P.Month_Id < Month_Id).OrderByDescending(O => O.Month_Id).Take(12).ToList();
                        
            double Intercept = 0;
            double Slope = 0;
            double Absolute_Quantity_Forcast = 0;
            double Quantity_Forcast = 0;
            string JSON_MonthlyTotals = JsonConvert.SerializeObject(Totals);
            int Sample_Size = Totals.Count();
            string SkuClass = "Unknown";
            bool Valid = false;
            if (Sample_Size > 3)
            {
                Valid = true;
                double[] YTotals = Totals.Select(P => P.Absolute_Quantity_Sold).ToArray();
                double[] XMonth = Totals.Select(P => (double)P.Month_Id).ToArray();
                double[] t = Linear.Forcast(Month_Id, XMonth, YTotals);
                Intercept = t[0];
                Slope = t[1];
                Absolute_Quantity_Forcast = t[2];
                Quantity_Forcast = MonthlyTotal.AddSeasonality(Absolute_Quantity_Forcast, month);
                SkuClass = SingleLinearForcast.GetSkuClass((int)Totals.Select(P => P.Quantity_Sold).ToArray().Sum());
            }
            SingleLinearForcast Forcast = new SingleLinearForcast()
            {
                SKU = id,
                Month_Id = Month_Id, //Month * Year
                Date = ForcastDate, //Forcasted Date
                Quantity_Forcast = Quantity_Forcast,
                Absolute_Quantity_Forcast = Absolute_Quantity_Forcast,
                Slope = Slope,
                Intercept = Intercept,
                JSON_MonthlyTotals = JSON_MonthlyTotals,
                Sample_Size = Sample_Size,
                SkuClass = SkuClass,
                Valid = Valid,
            };
            db.SingleLinearForcasts.Add(Forcast);
            db.SaveChanges();
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