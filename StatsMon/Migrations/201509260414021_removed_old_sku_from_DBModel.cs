namespace StatsMon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removed_old_sku_from_DBModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SalesOrderDetails", "SalesOrderID", "dbo.SalesOrders");
            DropPrimaryKey("dbo.SalesOrders");
            AlterColumn("dbo.SalesOrders", "SalesOrderID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.SalesOrders", "SalesOrderID");
            AddForeignKey("dbo.SalesOrderDetails", "SalesOrderID", "dbo.SalesOrders", "SalesOrderID", cascadeDelete: true);
            DropTable("dbo.SKUInventoryStatistics");
            DropTable("dbo.SKUPurchases");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SKUPurchases",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        PurchaseID = c.Long(nullable: false),
                        Quantity = c.Long(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.PurchaseID });
            
            CreateTable(
                "dbo.SKUInventoryStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TwelveMonthSales = c.Long(nullable: false),
                        TwelveMonthType = c.Int(nullable: false),
                        SixMonthTrend = c.Double(nullable: false),
                        TwelveMonthAverage = c.Double(nullable: false),
                        SixMonthTrendInteger = c.Int(nullable: false),
                        SixMonthStandardDeviation = c.Double(nullable: false),
                        SixMonthPurchaseAffinity = c.String(),
                        CurrentForcast = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.SalesOrderDetails", "SalesOrderID", "dbo.SalesOrders");
            DropPrimaryKey("dbo.SalesOrders");
            AlterColumn("dbo.SalesOrders", "SalesOrderID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.SalesOrders", "SalesOrderID");
            AddForeignKey("dbo.SalesOrderDetails", "SalesOrderID", "dbo.SalesOrders", "SalesOrderID", cascadeDelete: true);
        }
    }
}
