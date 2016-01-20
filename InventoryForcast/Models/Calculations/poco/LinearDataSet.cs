using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryForcast.Models.Calculations.poco
{
    public class LinearDataSet
    {
        public string label { get; set; }
        public double[] x { get; set; }
        public double[] y { get; set; }
    }
}
