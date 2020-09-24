namespace WebSQLEntityCodeFirst.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseID = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        Title = c.String(),
                        Credits = c.Int(nullable: false),
                        Quota = c.Int(nullable: false),
                        CurrentQuota = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CourseID);
            
            CreateTable(
                "dbo.Electives",
                c => new
                    {
                        EnrollmentID = c.Int(nullable: false, identity: true),
                        CourseID = c.Int(nullable: false),
                        StudentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EnrollmentID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .Index(t => t.CourseID)
                .Index(t => t.StudentID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Gender = c.Int(nullable: false),
                        IDNo = c.String(nullable: false),
                        Class = c.String(),
                        Department = c.String(),
                        EnrollmentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Electives", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Electives", "CourseID", "dbo.Courses");
            DropIndex("dbo.Electives", new[] { "StudentID" });
            DropIndex("dbo.Electives", new[] { "CourseID" });
            DropTable("dbo.Students");
            DropTable("dbo.Electives");
            DropTable("dbo.Courses");
        }
    }
}
