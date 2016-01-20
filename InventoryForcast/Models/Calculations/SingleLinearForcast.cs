using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryForcast.Models.Calculations
{
    public class SingleLinearForcast
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SKU { get; set; }
        public int Month_Id { get; set; } //Month * Year
        public DateTime Date { get; set; } //Forcasted Date
        public double Quantity_Forcast { get; set; }
        public double Absolute_Quantity_Forcast { get; set; }
        public double Slope { get; set; }
        public double Intercept { get; set; }
        public string JSON_MonthlyTotals { get; set; }
        public int Sample_Size { get; set; }
        public string SkuClass { get; set; }
        public bool Valid { get; set; } //Used if not enough data exists to properly generate this calculation.
        public int SuggestedInventory {
            get {
                double serviceLevel = 0;
                switch (SkuClass)
                {
                    case "A":
                        serviceLevel = 1.2;
                        break;
                    case "B":
                        serviceLevel = 1;
                        break;
                    case "C":
                        serviceLevel = .8;
                        break;
                    default:
                        break;
                }
                double inventoryAmount = Math.Round(Quantity_Forcast * serviceLevel, 0, MidpointRounding.AwayFromZero);
                return Convert.ToInt32(inventoryAmount);
            }
        }
        public static string GetSkuClass(int TotalSales)
        {
            string SkuClass;
            switch (TotalSales)
            {
                case 0:
                case 1:
                    SkuClass = "A";
                    break;
                case 2:
                case 3:
                    SkuClass = "B";
                    break;
                case 4:
                case 5:
                    SkuClass = "C";
                    break;
                case 6:
                    SkuClass = "D6";
                    break;
                case 7:
                    SkuClass = "D5";
                    break;
                case 8:
                    SkuClass = "D4";
                    break;
                case 9:
                    SkuClass = "D3";
                    break;
                case 10:
                    SkuClass = "D2";
                    break;
                case 11:
                    SkuClass = "D1";
                    break;
                default:
                    SkuClass = "E";
                    break;
            }
            return SkuClass;
        }
    }
}