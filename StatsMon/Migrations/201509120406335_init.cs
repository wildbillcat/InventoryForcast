namespace StatsMon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SKUPurchases",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        PurchaseID = c.Long(nullable: false),
                        Quantity = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.PurchaseID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SKUPurchases");
        }
    }
}
