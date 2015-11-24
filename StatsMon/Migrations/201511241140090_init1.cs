namespace StatsMon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.InventoryReports");
            AlterColumn("dbo.InventoryReports", "SkuId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.InventoryReports", "SkuId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.InventoryReports");
            AlterColumn("dbo.InventoryReports", "SkuId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.InventoryReports", "SkuId");
        }
    }
}
