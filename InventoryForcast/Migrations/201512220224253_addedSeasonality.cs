namespace InventoryForcast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedSeasonality : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SingleLinearForcasts",
                c => new
                    {
                        SKU = c.Int(nullable: false, identity: false),
                        Month_Id = c.Int(nullable: false, identity: false),
                        Date = c.DateTime(nullable: false),
                        Quantity_Forcast = c.Double(nullable: false),
                        Absolute_Quantity_Forcast = c.Double(nullable: false),
                        Slope = c.Double(nullable: false),
                        Intercept = c.Double(nullable: false),
                        JSON_MonthlyTotals = c.String(),
                        Sample_Size = c.Int(nullable: false),
                        SkuClass = c.String(),
                        Valid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SKU, t.Month_Id });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SingleLinearForcasts");
        }
    }
}
