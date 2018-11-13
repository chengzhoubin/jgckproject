namespace JGCK.Repority.OrderWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initalCreate : DbMigration
    {
        public override void Up()
        {
            /*
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        OrderNO = c.String(nullable: false, maxLength: 100, unicode: false),
                        ProcessingType = c.Int(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        Desc = c.String(maxLength: 500),
                        FinishTime = c.DateTime(),
                        PatientId = c.Long(),
                        PersonId = c.Long(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Patient", t => t.PatientId)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.OrderAttachFile",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        FilePath = c.String(maxLength: 300),
                        OrderId = c.Long(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Order", t => t.OrderId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderProduct",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProductCount = c.Long(),
                        Price = c.Decimal(nullable: false, precision: 2, scale: 0),
                        OrderId = c.Long(nullable: false),
                        ProductId = c.Long(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Order", t => t.OrderId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderTrace",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Comment = c.String(maxLength: 200),
                        PersonId = c.Long(nullable: false),
                        OrderId = c.Long(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Order", t => t.OrderId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Patient",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Birthday = c.DateTime(),
                        Sex = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            */
        }
        
        public override void Down()
        {
            /*
            DropForeignKey("dbo.Order", "PatientId", "dbo.Patient");
            DropForeignKey("dbo.OrderTrace", "OrderId", "dbo.Order");
            DropForeignKey("dbo.OrderProduct", "OrderId", "dbo.Order");
            DropForeignKey("dbo.OrderAttachFile", "OrderId", "dbo.Order");
            DropIndex("dbo.OrderTrace", new[] { "OrderId" });
            DropIndex("dbo.OrderProduct", new[] { "OrderId" });
            DropIndex("dbo.OrderAttachFile", new[] { "OrderId" });
            DropIndex("dbo.Order", new[] { "PatientId" });
            DropTable("dbo.Patient");
            DropTable("dbo.OrderTrace");
            DropTable("dbo.OrderProduct");
            DropTable("dbo.OrderAttachFile");
            DropTable("dbo.Order");
            */
        }
    }
}
