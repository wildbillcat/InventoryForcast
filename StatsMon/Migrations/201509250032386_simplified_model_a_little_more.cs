namespace StatsMon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class simplified_model_a_little_more : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SalesOrders", "SalesOrderNumber", c => c.String(nullable: false, maxLength: 25));
            DropColumn("dbo.SalesOrders", "RevisionNumber");
            DropColumn("dbo.SalesOrders", "OnlineOrderFlag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SalesOrders", "OnlineOrderFlag", c => c.Boolean(nullable: false));
            AddColumn("dbo.SalesOrders", "RevisionNumber", c => c.Byte(nullable: false));
            AlterColumn("dbo.SalesOrders", "SalesOrderNumber", c => c.String(nullable: false, maxLength: 25));
        }
    }
}
