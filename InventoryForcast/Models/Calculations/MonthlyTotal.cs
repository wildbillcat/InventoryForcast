using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
                case 2:
                    return 1;
                case 3:
                    return 1.15;
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    return 1.265; //April, May, June, July, August.
                case 9:
                case 10:
                case 11:
                    return 1.1385;
                case 12:
                    return 1.02465;
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