namespace WebSQLEntityCodeFirst.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationRoles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        LastModifiedUserId = c.Int(nullable: false),
                        LastModifyDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Url = c.String(),
                        IsGranted = c.Boolean(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        RoleId = c.Int(nullable: false),
                        MenuItemsId = c.Int(),
                        ApplicationRoles_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationRoles", t => t.ApplicationRoles_ID)
                .ForeignKey("dbo.MenuItems", t => t.MenuItemsId)
                .Index(t => t.MenuItemsId)
                .Index(t => t.ApplicationRoles_ID);
            
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        IsGranted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LogonId = c.String(nullable: false),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        Email = c.String(),
                        Gender = c.Int(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        IDNo = c.String(nullable: false),
                        LockoutEnabled = c.Boolean(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        EnrollmentDate = c.DateTime(),
                        GraduationDate = c.DateTime(),
                        DropOutDate = c.DateTime(),
                        LeaveDate = c.DateTime(),
                        LastModifierUserId = c.Int(nullable: false),
                        LastModifierTime = c.DateTime(),
                        Identity = c.Int(nullable: false),
                        UserStateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserStates", t => t.UserStateId, cascadeDelete: true)
                .Index(t => t.UserStateId);
            
            CreateTable(
                "dbo.ChooseAClassrooms",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartingSchoolYear = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        NumberOfHours = c.Int(nullable: false),
                        ClassTimeId = c.Int(),
                        ClassroomId = c.Int(),
                        CourseId = c.Int(),
                        LogonId = c.String(),
                        ApplicationUserId = c.Int(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Classrooms", t => t.ClassroomId)
                .ForeignKey("dbo.ClassTimes", t => t.ClassTimeId)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.ClassTimeId)
                .Index(t => t.ClassroomId)
                .Index(t => t.CourseId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Classrooms",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SchoolNumber = c.String(),
                        ClassroomNumber = c.String(),
                        Location = c.String(),
                        Floor = c.String(),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        LastModifiedUserId = c.Int(nullable: false),
                        LastModifyDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ChooseAClassroomLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Remarks = c.String(),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        NumberOfHours = c.Int(nullable: false),
                        ClassTimeId = c.Int(),
                        ClassroomId = c.Int(),
                        CourseId = c.Int(),
                        LogonId = c.String(),
                        ApplicationUserId = c.Int(),
                        ChooseAClassroomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Classrooms", t => t.ClassroomId)
                .ForeignKey("dbo.ClassTimes", t => t.ClassTimeId)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.ClassTimeId)
                .Index(t => t.ClassroomId)
                .Index(t => t.CourseId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.ClassTimes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Rank = c.Int(nullable: false),
                        TimePeriod = c.String(),
                        Time = c.String(),
                        ClassPeriod = c.Int(nullable: false),
                        Week = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseID = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        SubjectNumber = c.String(),
                        Credits = c.Int(nullable: false),
                        StartingSchoolYear = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        Grade = c.Int(nullable: false),
                        Class = c.String(),
                        RequiredElective = c.Int(nullable: false),
                        NumberOfHours = c.Int(nullable: false),
                        HoursInternship = c.Int(nullable: false),
                        CourseDate = c.DateTime(nullable: false),
                        SignupBeginDate = c.DateTime(nullable: false),
                        SignupEndDate = c.DateTime(nullable: false),
                        MinNumber = c.Int(nullable: false),
                        MaxNumber = c.Int(nullable: false),
                        LogonId = c.String(),
                        Title = c.String(),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        LastModifiedUserId = c.Int(nullable: false),
                        LastModifyDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        CourseDescriptionId = c.Int(nullable: false),
                        SectionDepartmentId = c.Int(nullable: false),
                        ClassTimeId = c.Int(),
                        ClassroomId = c.Int(),
                        ApplicationUserId = c.Int(),
                    })
                .PrimaryKey(t => t.CourseID)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Classrooms", t => t.ClassroomId)
                .ForeignKey("dbo.ClassTimes", t => t.ClassTimeId)
                .ForeignKey("dbo.CourseDescriptions", t => t.CourseDescriptionId, cascadeDelete: true)
                .ForeignKey("dbo.SectionDepartments", t => t.SectionDepartmentId, cascadeDelete: true)
                .Index(t => t.CourseDescriptionId)
                .Index(t => t.SectionDepartmentId)
                .Index(t => t.ClassTimeId)
                .Index(t => t.ClassroomId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.CourseDescriptions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Objectives = c.String(),
                        CourseOutline = c.String(),
                        Textbooks = c.String(),
                        ReferenceBooks = c.String(),
                        Grading = c.String(),
                        Schedule = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CourseLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Remarks = c.String(),
                        Subject = c.String(nullable: false),
                        SubjectNumber = c.String(),
                        Credits = c.Int(nullable: false),
                        StartingSchoolYear = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        Grade = c.Int(nullable: false),
                        Class = c.String(),
                        RequiredElective = c.Int(nullable: false),
                        NumberOfHours = c.Int(nullable: false),
                        HoursInternship = c.Int(nullable: false),
                        CourseDate = c.DateTime(),
                        SignupBeginDate = c.DateTime(),
                        SignupEndDate = c.DateTime(),
                        MinNumber = c.Int(nullable: false),
                        MaxNumber = c.Int(nullable: false),
                        LogonId = c.String(),
                        Title = c.String(),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        CourseDescriptionId = c.Int(),
                        SectionDepartmentId = c.Int(),
                        ClassTimeId = c.Int(),
                        ClassroomId = c.Int(),
                        ApplicationUserId = c.Int(),
                        CourseId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Classrooms", t => t.ClassroomId)
                .ForeignKey("dbo.ClassTimes", t => t.ClassTimeId)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .ForeignKey("dbo.CourseDescriptions", t => t.CourseDescriptionId)
                .ForeignKey("dbo.SectionDepartments", t => t.SectionDepartmentId)
                .Index(t => t.CourseDescriptionId)
                .Index(t => t.SectionDepartmentId)
                .Index(t => t.ClassTimeId)
                .Index(t => t.ClassroomId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.SectionDepartments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Section = c.String(),
                        Department = c.String(),
                        DepartmentAbbreviation = c.String(),
                        CourseSorts = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CourseTimePerSemesters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartingSchoolYear = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        Grade = c.Int(nullable: false),
                        SignupBeginDate = c.DateTime(nullable: false),
                        SignupEndDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        LastModifiedUserId = c.Int(nullable: false),
                        LastModifyDate = c.DateTime(),
                        Section = c.String(),
                        SectionDepartment_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SectionDepartments", t => t.SectionDepartment_ID)
                .Index(t => t.SectionDepartment_ID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LogonId = c.String(nullable: false),
                        Annual = c.Int(nullable: false),
                        Grade = c.Int(nullable: false),
                        Class = c.String(),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        LastModifiedUserId = c.Int(nullable: false),
                        LastModifyDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                        SectionDepartmentId = c.Int(nullable: false),
                        UserStateId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.SectionDepartments", t => t.SectionDepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.UserStates", t => t.UserStateId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.SectionDepartmentId)
                .Index(t => t.UserStateId);
            
            CreateTable(
                "dbo.Electives",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartingSchoolYear = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        Credits = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        LogonId = c.String(),
                        StudentId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.CourseId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.UserStates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        InSchoolState = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CourseStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        CurrentQuota = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LogonId = c.String(nullable: false),
                        Annual = c.Int(nullable: false),
                        Tel = c.String(),
                        ResearchAreas = c.String(),
                        JobTitle = c.String(),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        LastModifiedUserId = c.Int(nullable: false),
                        LastModifyDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        ClassroomId = c.Int(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Classrooms", t => t.ClassroomId, cascadeDelete: true)
                .Index(t => t.ClassroomId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.ApplicationUserRoles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ElectiveLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Remarks = c.String(),
                        CreatedUserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        StartingSchoolYear = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        Credits = c.Int(nullable: false),
                        CourseId = c.Int(),
                        LogonId = c.String(),
                        StudentId = c.Int(),
                        ElectiveId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.CourseId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Homes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Contents = c.String(),
                        CreateUser = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ElectiveLogs", "StudentId", "dbo.Students");
            DropForeignKey("dbo.ElectiveLogs", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Employees", "ClassroomId", "dbo.Classrooms");
            DropForeignKey("dbo.Employees", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.CourseStatus", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Students", "UserStateId", "dbo.UserStates");
            DropForeignKey("dbo.ApplicationUsers", "UserStateId", "dbo.UserStates");
            DropForeignKey("dbo.Students", "SectionDepartmentId", "dbo.SectionDepartments");
            DropForeignKey("dbo.Electives", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Electives", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Students", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.CourseTimePerSemesters", "SectionDepartment_ID", "dbo.SectionDepartments");
            DropForeignKey("dbo.CourseLogs", "SectionDepartmentId", "dbo.SectionDepartments");
            DropForeignKey("dbo.Courses", "SectionDepartmentId", "dbo.SectionDepartments");
            DropForeignKey("dbo.CourseLogs", "CourseDescriptionId", "dbo.CourseDescriptions");
            DropForeignKey("dbo.CourseLogs", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CourseLogs", "ClassTimeId", "dbo.ClassTimes");
            DropForeignKey("dbo.CourseLogs", "ClassroomId", "dbo.Classrooms");
            DropForeignKey("dbo.CourseLogs", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Courses", "CourseDescriptionId", "dbo.CourseDescriptions");
            DropForeignKey("dbo.Courses", "ClassTimeId", "dbo.ClassTimes");
            DropForeignKey("dbo.Courses", "ClassroomId", "dbo.Classrooms");
            DropForeignKey("dbo.ChooseAClassroomLogs", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.ChooseAClassrooms", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ChooseAClassroomLogs", "ClassTimeId", "dbo.ClassTimes");
            DropForeignKey("dbo.ChooseAClassrooms", "ClassTimeId", "dbo.ClassTimes");
            DropForeignKey("dbo.ChooseAClassroomLogs", "ClassroomId", "dbo.Classrooms");
            DropForeignKey("dbo.ChooseAClassroomLogs", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ChooseAClassrooms", "ClassroomId", "dbo.Classrooms");
            DropForeignKey("dbo.ChooseAClassrooms", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Permissions", "MenuItemsId", "dbo.MenuItems");
            DropForeignKey("dbo.Permissions", "ApplicationRoles_ID", "dbo.ApplicationRoles");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ElectiveLogs", new[] { "StudentId" });
            DropIndex("dbo.ElectiveLogs", new[] { "CourseId" });
            DropIndex("dbo.Employees", new[] { "ApplicationUserId" });
            DropIndex("dbo.Employees", new[] { "ClassroomId" });
            DropIndex("dbo.CourseStatus", new[] { "CourseId" });
            DropIndex("dbo.Electives", new[] { "StudentId" });
            DropIndex("dbo.Electives", new[] { "CourseId" });
            DropIndex("dbo.Students", new[] { "UserStateId" });
            DropIndex("dbo.Students", new[] { "SectionDepartmentId" });
            DropIndex("dbo.Students", new[] { "ApplicationUserId" });
            DropIndex("dbo.CourseTimePerSemesters", new[] { "SectionDepartment_ID" });
            DropIndex("dbo.CourseLogs", new[] { "CourseId" });
            DropIndex("dbo.CourseLogs", new[] { "ApplicationUserId" });
            DropIndex("dbo.CourseLogs", new[] { "ClassroomId" });
            DropIndex("dbo.CourseLogs", new[] { "ClassTimeId" });
            DropIndex("dbo.CourseLogs", new[] { "SectionDepartmentId" });
            DropIndex("dbo.CourseLogs", new[] { "CourseDescriptionId" });
            DropIndex("dbo.Courses", new[] { "ApplicationUserId" });
            DropIndex("dbo.Courses", new[] { "ClassroomId" });
            DropIndex("dbo.Courses", new[] { "ClassTimeId" });
            DropIndex("dbo.Courses", new[] { "SectionDepartmentId" });
            DropIndex("dbo.Courses", new[] { "CourseDescriptionId" });
            DropIndex("dbo.ChooseAClassroomLogs", new[] { "ApplicationUserId" });
            DropIndex("dbo.ChooseAClassroomLogs", new[] { "CourseId" });
            DropIndex("dbo.ChooseAClassroomLogs", new[] { "ClassroomId" });
            DropIndex("dbo.ChooseAClassroomLogs", new[] { "ClassTimeId" });
            DropIndex("dbo.ChooseAClassrooms", new[] { "ApplicationUserId" });
            DropIndex("dbo.ChooseAClassrooms", new[] { "CourseId" });
            DropIndex("dbo.ChooseAClassrooms", new[] { "ClassroomId" });
            DropIndex("dbo.ChooseAClassrooms", new[] { "ClassTimeId" });
            DropIndex("dbo.ApplicationUsers", new[] { "UserStateId" });
            DropIndex("dbo.Permissions", new[] { "ApplicationRoles_ID" });
            DropIndex("dbo.Permissions", new[] { "MenuItemsId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Homes");
            DropTable("dbo.ElectiveLogs");
            DropTable("dbo.ApplicationUserRoles");
            DropTable("dbo.Employees");
            DropTable("dbo.CourseStatus");
            DropTable("dbo.UserStates");
            DropTable("dbo.Electives");
            DropTable("dbo.Students");
            DropTable("dbo.CourseTimePerSemesters");
            DropTable("dbo.SectionDepartments");
            DropTable("dbo.CourseLogs");
            DropTable("dbo.CourseDescriptions");
            DropTable("dbo.Courses");
            DropTable("dbo.ClassTimes");
            DropTable("dbo.ChooseAClassroomLogs");
            DropTable("dbo.Classrooms");
            DropTable("dbo.ChooseAClassrooms");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.MenuItems");
            DropTable("dbo.Permissions");
            DropTable("dbo.ApplicationRoles");
        }
    }
}
