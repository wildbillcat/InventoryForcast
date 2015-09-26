namespace StatsMon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updated_identity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SalesOrderDetails", "SalesOrderID", "dbo.SalesOrders");
            DropPrimaryKey("dbo.SalesOrders");
            AlterColumn("dbo.SalesOrders", "SalesOrderID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.SalesOrders", "SalesOrderID");
            AddForeignKey("dbo.SalesOrderDetails", "SalesOrderID", "dbo.SalesOrders", "SalesOrderID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesOrderDetails", "SalesOrderID", "dbo.SalesOrders");
            DropPrimaryKey("dbo.SalesOrders");
            AlterColumn("dbo.SalesOrders", "SalesOrderID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.SalesOrders", "SalesOrderID");
            AddForeignKey("dbo.SalesOrderDetails", "SalesOrderID", "dbo.SalesOrders", "SalesOrderID", cascadeDelete: true);
        }
    }
}
