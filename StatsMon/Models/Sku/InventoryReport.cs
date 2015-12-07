using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StatsMon.Models.Sku
{
    public class InventoryReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SkuId { get; set; }
        public double StandardDeviation { get; set; }
        public double ForcastSlope { get; set; }
        public double ForcastIntercept { get; set; }
        public double ForcastValue { get; set; }
        public int TotalSales { get; set; }
        public string SkuClass { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime UpperboundDate { get; set; }
        public string InternalData { get; set; }
        public int[] Last12Months
        {
            get
            {
                return Array.ConvertAll(InternalData.Split(';'), Int32.Parse); 
            }
            set
            {
                int[] _Last12Months = value;
                InternalData = String.Join(";", _Last12Months.Select(p => p.ToString()).ToArray());
            }
        }
        public InventoryReport() { }

        public InventoryReport(int _SkuId, double _StandardDeviation, double[] _ForcastVals, int _TotalSales, int[] _Last12Months, DateTime _Upperbound)
        {
            LastUpdated = DateTime.Now;
            SkuId = _SkuId;
            StandardDeviation = _StandardDeviation;
            ForcastSlope = _ForcastVals[0];
            ForcastIntercept = _ForcastVals[1];
            ForcastValue = _ForcastVals[2];
            TotalSales = _TotalSales;
            Last12Months = _Last12Months;
            UpperboundDate = _Upperbound;
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
        }
    }
}