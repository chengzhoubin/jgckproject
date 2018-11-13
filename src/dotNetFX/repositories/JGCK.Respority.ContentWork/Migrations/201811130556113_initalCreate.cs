namespace JGCK.Respority.ContentWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initalCreate : DbMigration
    {
        public override void Up()
        {
            /*
            CreateTable(
                "dbo.PortalColumn",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        ColumnType = c.Int(),
                        Status = c.Int(),
                        Content = c.String(maxLength: 500),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            */
        }
        
        public override void Down()
        {
            /*
            DropTable("dbo.PortalColumn");
            */
        }
    }
}
