namespace JGCK.Respority.BasicInfo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initalCreate : DbMigration
    {
        public override void Up()
        {
            /*
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Desc = c.String(maxLength: 200),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Hospital",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Address = c.String(maxLength: 150),
                        Grade = c.String(maxLength: 20),
                        ContactNO = c.String(maxLength: 50),
                        SaleRate = c.Decimal(precision: 2, scale: 0),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OffDay",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        NonworkDate = c.DateTime(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
                */
            
        }
        
        public override void Down()
        {
            /*
            DropTable("dbo.OffDay");
            DropTable("dbo.Hospital");
            DropTable("dbo.Department");
            */
        }
    }
}
