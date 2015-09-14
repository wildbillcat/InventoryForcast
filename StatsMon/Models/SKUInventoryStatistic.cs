using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatsMon.Models
{
    public enum SKUInventoryType { A, B, C, D1, D2, D3, D4, D5, D6, E }

    public class SKUInventoryStatistic
    {
        //SKU ID	12 Month Sales	Type	trend 	average	trend intiger	ST Dev
        [Column(Order = 0), Key]
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// The current total number of sales over twelves months
        /// </summary>
        public long TwelveMonthSales { get; set; }

        /// <summary>
        /// This has some sort of meaning (12 Months)
        /// </summary>
        public SKUInventoryType TwelveMonthType { get; set; }

        /// <summary>
        /// Current Sales Trend exhibited (6 Months)
        /// </summary>
        public double SixMonthTrend { get; set; }

        /// <summary>
        /// LifeTime Average Sale of SKU (12 Month Average)
        /// </summary>
        public double TwelveMonthAverage { get; set; }

        /// <summary>
        /// Current Sales Trend exhibited (6 Months)
        /// </summary>
        public int SixMonthTrendInteger { get; set; }

        /// <summary>
        /// Standard Deviation over the past 6 months
        /// </summary>
        public double SixMonthStandardDeviation { get; set; }

        /// <summary>
        /// Affinity of other items that are frequently purchased with this item.
        /// </summary>
        public string SixMonthPurchaseAffinity { get; set; }

        /// <summary>
        /// This is the presently calculated forcast based on the sales data
        /// </summary>
        public int CurrentForcast { get; set; }
    }
}