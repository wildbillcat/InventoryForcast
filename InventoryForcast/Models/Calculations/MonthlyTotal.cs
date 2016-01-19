using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace InventoryForcast.Models.Calculations
{
    public class MonthlyTotal
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SKU { get; set; }
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Month_Id { get; set; } //Month * Year
        public double Quantity_Sold { get; set; }
        public double Absolute_Quantity_Sold { get; set; }
        public DateTime Date { get; set; }

        public static double GetSeasonality(int month)
        {
            switch (month){
                case 1:
                    return double.Parse(ConfigurationManager.AppSettings.Get("JanuarySeasonality"));
                case 2:
                    return double.Parse(ConfigurationManager.AppSettings.Get("FebruarySeasonality"));
                case 3:
                    return double.Parse(ConfigurationManager.AppSettings.Get("MarchSeasonality"));
                case 4:
                    return double.Parse(ConfigurationManager.AppSettings.Get("AprilSeasonality"));
                case 5:
                    return double.Parse(ConfigurationManager.AppSettings.Get("MaySeasonality"));
                case 6:
                    return double.Parse(ConfigurationManager.AppSettings.Get("JuneSeasonality"));
                case 7:
                    return double.Parse(ConfigurationManager.AppSettings.Get("JulySeasonality"));
                case 8:
                    return double.Parse(ConfigurationManager.AppSettings.Get("AugustSeasonality"));
                case 9:
                    return double.Parse(ConfigurationManager.AppSettings.Get("SeptemberSeasonality"));
                case 10:
                    return double.Parse(ConfigurationManager.AppSettings.Get("OctoberSeasonality"));
                case 11:
                    return double.Parse(ConfigurationManager.AppSettings.Get("NovemberSeasonality"));
                case 12:
                    return double.Parse(ConfigurationManager.AppSettings.Get("DecemberSeasonality"));
                default:
                    throw new Exception(); 
            }
        }

        public static double AddSeasonality(double qty, int month) {
            return qty * GetSeasonality(month);
        }

        public static double RemoveSeasonality(double qty, int month)
        {
            return qty / GetSeasonality(month);
        }
    }
}