using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryForcast.Models.Calculations
{
    public class ForcastValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SKU { get; set; }
        public double Quantity_Forcast { get; set; }
        public double Absolute_Quantity_Sold { get; set; }
        public DateTime Date { get; set; } //Forcasted Date
        public int Month_Id { get; set; } //Forcasted Date
        public double Slope { get; set; }
        public double Intercept { get; set; }
        public string JSON_MonthlyTotals { get; set; }
        public int Sample_Size { get; set; }
        public bool Valid { get; set; } //Used if not enough data exists to properly generate this calculation.
    }
}