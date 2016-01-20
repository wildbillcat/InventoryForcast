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
using InventoryForcast.Models.Calculations.poco;

namespace InventoryForcast.Models.Calculations.Generators
{
    public static class SingleLinearForcastGenerator
    {
        public static void GenerateSingleLinearForcast(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MonthlyTotal MT = db.MonthlyTotals.Where(p => p.SKU == id).OrderBy(g => g.Date).ToArray().Last();
            DateTime ForcastDate = MT.Date.AddMonths(1);
            List<MonthlyTotal> Totals = db.MonthlyTotals.Where(P => P.SKU == id && P.Date < ForcastDate).OrderByDescending(O => O.Date).Take(12).ToList();
            List<LinearDataSet> DS = new List<LinearDataSet>();
            double Intercept = 0;
            double Slope = 0;
            double Absolute_Quantity_Forcast = 0;
            double Quantity_Forcast = 0;
            string JSON_MonthlyTotals = JsonConvert.SerializeObject(DS.ToArray());
            int Sample_Size = Totals.Count();
            string SkuClass = "Unknown";
            bool Valid = false;
            if (Sample_Size > 3)
            {
                Valid = true;
                double[] YTotals = Totals.Select(P => P.Absolute_Quantity_Sold).ToArray();
                double[] YTotalsR = Totals.Select(P => P.Quantity_Sold).ToArray();
                double[] XMonth = Totals.Select(P => (double)(P.Date.Year * 12) + P.Date.Month).ToArray();
                double[] t = Linear.Forcast(ForcastDate.Month, XMonth, YTotals);
                DS.Add(new LinearDataSet() { label = "Actual Sales", y = YTotalsR, x = XMonth });
                DS.Add(new LinearDataSet() { label = "Adjusted Sales", y = YTotals, x = XMonth });
                List<double> TrendYVals = new List<double>();
                List<double> TrendYValsSeasonal = new List<double>();
                foreach (double x in XMonth)
                {
                    double val = x * t[1] + t[0];
                    TrendYVals.Add(val);
                }
                DS.Add(new LinearDataSet()
                {
                    label = "Trend Sales",
                    y = TrendYVals.ToArray(),
                    x = XMonth
                });
                JSON_MonthlyTotals = JsonConvert.SerializeObject(DS.ToArray());
                Intercept = t[0];
                Slope = t[1];
                Absolute_Quantity_Forcast = t[2];
                Quantity_Forcast = MonthlyTotal.AddSeasonality(Absolute_Quantity_Forcast, ForcastDate.Month);
                SkuClass = SingleLinearForcast.GetSkuClass((int)Totals.Select(P => P.Quantity_Sold).ToArray().Sum());
            }
            SingleLinearForcast Forcast = new SingleLinearForcast()
            {
                SKU = id,
                Month_Id = (12 * ForcastDate.Year) + ForcastDate.Month, //Month * Year
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
    }
}