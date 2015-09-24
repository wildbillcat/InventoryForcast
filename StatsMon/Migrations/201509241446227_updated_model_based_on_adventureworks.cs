namespace StatsMon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updated_model_based_on_adventureworks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalesOrderDetails",
                c => new
                    {
                        SalesOrderID = c.Int(nullable: false),
                        SalesOrderDetailID = c.Int(nullable: false),
                        CarrierTrackingNumber = c.String(maxLength: 25),
                        OrderQty = c.Short(nullable: false),
                        ProductID = c.Int(nullable: false),
                        SpecialOfferID = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, storeType: "money"),
                        UnitPriceDiscount = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => new { t.SalesOrderID, t.SalesOrderDetailID })
                .ForeignKey("dbo.SalesOrders", t => t.SalesOrderID, cascadeDelete: true)
                .Index(t => t.SalesOrderID);
            
            CreateTable(
                "dbo.SalesOrders",
                c => new
                    {
                        SalesOrderID = c.Int(nullable: false, identity: true),
                        RevisionNumber = c.Byte(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        ShipDate = c.DateTime(),
                        Status = c.Byte(nullable: false),
                        OnlineOrderFlag = c.Boolean(nullable: false),
                        SalesOrderNumber = c.String(nullable: false, maxLength: 25),
                        PurchaseOrderNumber = c.String(maxLength: 25),
                        AccountNumber = c.String(maxLength: 15),
                        CustomerID = c.Int(nullable: false),
                        TerritoryID = c.Int(),
                        BillToAddressID = c.Int(nullable: false),
                        ShipToAddressID = c.Int(nullable: false),
                        ShipMethodID = c.Int(nullable: false),
                        CreditCardID = c.Int(),
                        CreditCardApprovalCode = c.String(maxLength: 15, unicode: false),
                        CurrencyRateID = c.Int(),
                        SubTotal = c.Decimal(nullable: false, storeType: "money"),
                        TaxAmt = c.Decimal(nullable: false, storeType: "money"),
                        Freight = c.Decimal(nullable: false, storeType: "money"),
                        Comment = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SalesOrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesOrderDetails", "SalesOrderID", "dbo.SalesOrders");
            DropIndex("dbo.SalesOrderDetails", new[] { "SalesOrderID" });
            DropTable("dbo.SalesOrders");
            DropTable("dbo.SalesOrderDetails");
        }
    }
}
