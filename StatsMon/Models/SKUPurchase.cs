using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatsMon.Models
{
    public class SKUPurchase
    {
        /// <summary>
        /// SKU ID Number
        /// </summary>
        [Column(Order = 0), Key]
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// ID Number of the affiliated Purchase
        /// </summary>
        [Column(Order = 1), Key]
        [Required]
        public long PurchaseID { get; set; }

        /// <summary>
        /// Quantity of SKU bough in Purchase
        /// </summary>
        public long Quantity { get; set; }

        /// <summary>
        /// Date of the SKU Purchase
        /// </summary>
        public DateTime PurchaseDate { get; set; }
    }
}