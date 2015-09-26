namespace StatsMon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updated_identity_on_details : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SalesOrderDetails", new[] { "SalesOrderID" });
            DropPrimaryKey("dbo.SalesOrderDetails");
            AlterColumn("dbo.SalesOrderDetails", "SalesOrderID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.SalesOrderDetails", "SalesOrderDetailID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.SalesOrderDetails", new[] { "SalesOrderID", "SalesOrderDetailID" });
            CreateIndex("dbo.SalesOrderDetails", "SalesOrderID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SalesOrderDetails", new[] { "SalesOrderID" });
            DropPrimaryKey("dbo.SalesOrderDetails");
            AlterColumn("dbo.SalesOrderDetails", "SalesOrderDetailID", c => c.Int(nullable: false));
            AlterColumn("dbo.SalesOrderDetails", "SalesOrderID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.SalesOrderDetails", new[] { "SalesOrderID", "SalesOrderDetailID" });
            CreateIndex("dbo.SalesOrderDetails", "SalesOrderID");
        }
    }
}
