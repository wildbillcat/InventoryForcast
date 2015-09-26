﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Migrations;
using StatsMon.Models.Sales;

namespace StatsMon.Models
{
    public class StatusMonContext : DbContext
    {
        public StatusMonContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<StatusMonContext>(new StatusMonContextInitializer());
        }

        public StatusMonContext(string connectionString)
            : base(connectionString)
        {
            //Database.SetInitializer<StatusMonContext>(new StatusMonContextInitializer());
        }

        public DbSet<SKUPurchase> SKUPurchases { get; set; }
        public DbSet<SKUInventoryStatistic> SKUInventoryStatistics { get; set; }

        public virtual DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }
        public virtual DbSet<SalesOrder> SalesOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesOrderDetail>()
                .Property(e => e.UnitPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SalesOrderDetail>()
                .Property(e => e.UnitPriceDiscount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SalesOrder>()
                .Property(e => e.CreditCardApprovalCode)
                .IsUnicode(false);

            modelBuilder.Entity<SalesOrder>()
                .Property(e => e.SubTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SalesOrder>()
                .Property(e => e.TaxAmt)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SalesOrder>()
                .Property(e => e.Freight)
                .HasPrecision(19, 4);

        }

    }

    public class StatusMonContextInitializer : DropCreateDatabaseAlways<StatusMonContext>
    {
        protected override void Seed(StatusMonContext context)
        {
            Random gen = new Random();
            for (int i = 0; i < 10; i++)
            {
                context.SalesOrders.AddOrUpdate(
                    new SalesOrder {
                        SalesOrderID = 1,
                        OrderDate = DateTime.Now,
                        Status = (byte)gen.Next(),
                        SalesOrderNumber = "Sdfcdfg435",
                        PurchaseOrderNumber = "POsdfsdfdg45",
                        AccountNumber = "ACC04rjvv8r",
                        CustomerID = 1,
                        TerritoryID = 1,
                        BillToAddressID = 1,
                        ShipToAddressID =1,
                        ShipMethodID =1,
                        CreditCardID = 1,
                        CreditCardApprovalCode = "TopPlastic",
                        CurrencyRateID = 0,
                        SubTotal = (decimal)22.5,
                        TaxAmt = 0,
                        Freight = 0,
                        Comment = ""
                        }
                    );

                context.SKUPurchases.AddOrUpdate(
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },//1825 is 8 years in days
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) }
                );
            }
            //base.Seed(context);
        }
    }
}