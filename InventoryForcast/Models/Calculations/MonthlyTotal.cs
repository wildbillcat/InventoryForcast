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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SKU { get; set; }
        public double Quantity_Sold { get; set; }
        public double Absolute_Quantity_Sold { get; set; }
        public DateTime Date { get; set; }
        public int Month_Id { get; set; }
    }
}