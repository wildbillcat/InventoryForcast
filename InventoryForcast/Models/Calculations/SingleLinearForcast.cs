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
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SKU { get; set; }
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
    }
}