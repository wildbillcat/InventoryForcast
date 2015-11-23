namespace StatsMon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removed_uneeded_upperbound : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InventoryReports", "ForcastRangeUpperBound");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InventoryReports", "ForcastRangeUpperBound", c => c.Double(nullable: false));
        }
    }
}
