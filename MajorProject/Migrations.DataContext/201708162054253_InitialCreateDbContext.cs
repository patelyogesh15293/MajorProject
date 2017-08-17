namespace MajorProject.Migrations.DataContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreateDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        ClassId = c.String(nullable: false, maxLength: 128, defaultValueSql: "newid()"),
                        ClassName = c.String(nullable: false, maxLength: 100),
                        ClassDescription = c.String(maxLength: 250),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                    EditDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                })
                .PrimaryKey(t => t.ClassId);
            
            CreateTable(
                "dbo.Signups",
                c => new
                    {
                        SignupId = c.String(nullable: false, maxLength: 128, defaultValueSql: "newid()"),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        ClassId = c.String(nullable: false, maxLength: 128),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                    EditDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                })
                .PrimaryKey(t => t.SignupId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .ForeignKey("dbo.Classes", t => t.ClassId)
                .Index(t => t.StudentId)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.String(nullable: false, maxLength: 128, defaultValueSql: "newid()"),
                        StudentName = c.String(nullable: false, maxLength: 250),
                        StudentPhone = c.String(maxLength: 50),
                        StudentEmail = c.String(maxLength: 50),
                        CreateDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                        EditDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                    })
                .PrimaryKey(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Signups", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.Signups", "StudentId", "dbo.Students");
            DropIndex("dbo.Signups", new[] { "ClassId" });
            DropIndex("dbo.Signups", new[] { "StudentId" });
            DropTable("dbo.Students");
            DropTable("dbo.Signups");
            DropTable("dbo.Classes");
        }
    }
}
