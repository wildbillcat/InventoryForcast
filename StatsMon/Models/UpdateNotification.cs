using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatsMon.Models
{
    public class UpdateNotification
    {
        public string UpdateType; //Add/Remove/Update

        public string ObjectType; //SKUPurchase/SKUInventoyStatistic

        public string ObjectKey; //1/12,24    Use Comma as delimiter when primary key is multiple fields
    }
}