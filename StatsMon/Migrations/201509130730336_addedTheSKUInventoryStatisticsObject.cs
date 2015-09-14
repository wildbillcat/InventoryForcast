namespace StatsMon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedTheSKUInventoryStatisticsObject : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SKUInventoryStatistics");
        }
    }
}
