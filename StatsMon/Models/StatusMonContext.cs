using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Migrations;

namespace StatsMon.Models
{
    public class StatusMonContext : DbContext
    {
        public StatusMonContext()
            : base("SchoolContext")
        {
            //Database.SetInitializer<StatusMonContext>(new StatusMonContextInitializer());
        }

        public DbSet<SKUPurchase> SKUPurchases { get; set; }
        public DbSet<SKUInventoryStatistic> SKUInventoryStatistics { get; set; }
    }

    public class StatusMonContextInitializer : DropCreateDatabaseAlways<StatusMonContext>
    {
        protected override void Seed(StatusMonContext context)
        {
            Random gen = new Random();
            for (int i = 0; i < 1000; i++)
            {
                context.SKUPurchases.AddOrUpdate(
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },//1825 is 8 years in days
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) },
                  new SKUPurchase { Id = gen.Next(411699), PurchaseID = gen.Next(), Quantity = gen.Next(1, 10), PurchaseDate = DateTime.UtcNow.AddDays(-1 * (new Random().Next(1825))) }
                );
            }
            //base.Seed(context);
        }
    }
}