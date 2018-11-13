namespace JGCK.Respority.UserWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initalCreate : DbMigration
    {
        public override void Up()
        {
            /*
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        HeadPicture = c.String(maxLength: 300),
                        StaffNO = c.String(maxLength: 50),
                        Sex = c.Boolean(nullable: false),
                        Position = c.String(maxLength: 100),
                        Account = c.String(maxLength: 50),
                        Pwd = c.String(maxLength: 50, unicode: false),
                        Email = c.String(maxLength: 100),
                        ContactNO = c.String(maxLength: 50, unicode: false),
                        PersonType = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DepartmentId = c.Long(),
                        RoleId = c.Long(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
                */
            
        }
        
        public override void Down()
        {
            /*
            DropForeignKey("dbo.Person", "RoleId", "dbo.Role");
            DropIndex("dbo.Person", new[] { "RoleId" });
            DropTable("dbo.Role");
            DropTable("dbo.Person");
            */
        }
    }
}
