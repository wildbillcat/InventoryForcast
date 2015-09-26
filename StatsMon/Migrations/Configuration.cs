namespace StatsMon.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using StatsMon.Models;
    using StatsMon.Models.Sales;

    internal sealed class Configuration : DbMigrationsConfiguration<StatsMon.Models.StatusMonContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StatsMon.Models.StatusMonContext context)
        {
            Random gen = new Random();
            for (int i = 0; i < 10; i++)
            {
                context.SalesOrders.AddOrUpdate(
                    new SalesOrder
                    {
                        SalesOrderID = gen.Next(),
                        OrderDate = DateTime.Now,
                        Status = (byte)gen.Next(),
                        SalesOrderNumber = "Sdfcdfg435",
                        PurchaseOrderNumber = "POsdfsdfdg45",
                        AccountNumber = "ACC04rjvv8r",
                        CustomerID = 1,
                        TerritoryID = 1,
                        BillToAddressID = 1,
                        ShipToAddressID = 1,
                        ShipMethodID = 1,
                        CreditCardID = 1,
                        CreditCardApprovalCode = "TopPlastic",
                        CurrencyRateID = 0,
                        SubTotal = (decimal)22.5,
                        TaxAmt = 0,
                        Freight = 0,
                        Comment = ""
                    }
                    );

            }
        }
    }
}
