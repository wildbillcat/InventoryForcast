namespace InventoryForcast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactor_key : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.SingleLinearForcasts");
            AddPrimaryKey("dbo.SingleLinearForcasts", "SKU");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.SingleLinearForcasts");
            AddPrimaryKey("dbo.SingleLinearForcasts", new[] { "SKU", "Month_Id" });
        }
    }
}
