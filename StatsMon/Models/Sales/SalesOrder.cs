using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatsMon.Models.Sales
{
    public class SalesOrder
    {
        [Column(Order = 0), Key]
        [Required]
        public long Id { get; set; }
    }
}