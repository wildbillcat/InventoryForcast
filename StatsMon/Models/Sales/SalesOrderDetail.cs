namespace StatsMon.Models.Sales
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SalesOrderDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesOrderID { get; set; }

        /// <summary>
        /// Normally this would be database generated in a Prod Environment, but the use
        /// case for this application is that it is a Data Warehouse application, thus
        /// Database would have already generated this ID.
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesOrderDetailID { get; set; }

        [StringLength(25)]
        public string CarrierTrackingNumber { get; set; }

        public short OrderQty { get; set; }

        public int ProductID { get; set; }

        public int SpecialOfferID { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPriceDiscount { get; set; }

        //[Column(TypeName = "numeric")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public decimal LineTotal { get; set; }
        
        public virtual SalesOrder SalesOrder { get; set; }
    }
}
