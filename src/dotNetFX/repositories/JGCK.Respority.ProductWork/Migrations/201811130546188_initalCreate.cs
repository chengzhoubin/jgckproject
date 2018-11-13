namespace JGCK.Respority.ProductWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initalCreate : DbMigration
    {
        public override void Up()
        {
            /*
            CreateTable(
                "dbo.BatchNumberInfo",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        BatchNO = c.String(nullable: false, maxLength: 100, unicode: false),
                        Desc = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                        MaterialId = c.Long(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Material", t => t.MaterialId)
                .Index(t => t.MaterialId);
            
            CreateTable(
                "dbo.Material",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        MaterialNO = c.String(maxLength: 100, unicode: false),
                        Name = c.String(nullable: false, maxLength: 150),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductBatchNumber",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProductId = c.Long(nullable: false),
                        BatchNumberInfoId = c.Long(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .ForeignKey("dbo.BatchNumberInfo", t => t.BatchNumberInfoId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.BatchNumberInfoId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProductNO = c.String(maxLength: 100, unicode: false),
                        Name = c.String(nullable: false, maxLength: 150),
                        BeginDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        VersionNO = c.String(maxLength: 100, unicode: false),
                        Price = c.Decimal(precision: 2, scale: 0),
                        Output = c.Long(),
                        CreaterId = c.Long(),
                        ProductTypeId = c.Long(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductTypeInfo", t => t.ProductTypeId)
                .Index(t => t.ProductTypeId);
            
            CreateTable(
                "dbo.ProductTypeInfo",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProductNO = c.String(maxLength: 100, unicode: false),
                        Name = c.String(nullable: false, maxLength: 150),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductionGroup",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            */
        }
        
        public override void Down()
        {
            /*
            DropForeignKey("dbo.ProductBatchNumber", "BatchNumberInfoId", "dbo.BatchNumberInfo");
            DropForeignKey("dbo.Product", "ProductTypeId", "dbo.ProductTypeInfo");
            DropForeignKey("dbo.ProductBatchNumber", "ProductId", "dbo.Product");
            DropForeignKey("dbo.BatchNumberInfo", "MaterialId", "dbo.Material");
            DropIndex("dbo.Product", new[] { "ProductTypeId" });
            DropIndex("dbo.ProductBatchNumber", new[] { "BatchNumberInfoId" });
            DropIndex("dbo.ProductBatchNumber", new[] { "ProductId" });
            DropIndex("dbo.BatchNumberInfo", new[] { "MaterialId" });
            DropTable("dbo.ProductionGroup");
            DropTable("dbo.ProductTypeInfo");
            DropTable("dbo.Product");
            DropTable("dbo.ProductBatchNumber");
            DropTable("dbo.Material");
            DropTable("dbo.BatchNumberInfo");
            */
        }
    }
}
