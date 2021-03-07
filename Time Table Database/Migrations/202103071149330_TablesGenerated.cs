namespace Time_Table_Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablesGenerated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        COMCOD = c.Int(nullable: false, identity: true),
                        CourseCode = c.String(),
                        CourseName = c.String(),
                        LectureUnits = c.Byte(nullable: false),
                        PracticalUnits = c.Byte(nullable: false),
                        TotalUnits = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.COMCOD);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SectionType = c.Int(nullable: false),
                        SectionNo = c.Int(nullable: false),
                        Room = c.Int(),
                        Course_COMCOD = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.Course_COMCOD)
                .Index(t => t.Course_COMCOD);
            
            CreateTable(
                "dbo.Timings",
                c => new
                    {
                        Day = c.Int(nullable: false),
                        Hour = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Day, t.Hour });
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SelectedEntries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Course_COMCOD = c.Int(),
                        MainSection_ID = c.Int(),
                        PracticalSection_ID = c.Int(),
                        TutorialSection_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.Course_COMCOD)
                .ForeignKey("dbo.Sections", t => t.MainSection_ID)
                .ForeignKey("dbo.Sections", t => t.PracticalSection_ID)
                .ForeignKey("dbo.Sections", t => t.TutorialSection_ID)
                .Index(t => t.Course_COMCOD)
                .Index(t => t.MainSection_ID)
                .Index(t => t.PracticalSection_ID)
                .Index(t => t.TutorialSection_ID);
            
            CreateTable(
                "dbo.TimingSections",
                c => new
                    {
                        Timing_Day = c.Int(nullable: false),
                        Timing_Hour = c.Int(nullable: false),
                        Section_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Timing_Day, t.Timing_Hour, t.Section_ID })
                .ForeignKey("dbo.Timings", t => new { t.Timing_Day, t.Timing_Hour }, cascadeDelete: true)
                .ForeignKey("dbo.Sections", t => t.Section_ID, cascadeDelete: true)
                .Index(t => new { t.Timing_Day, t.Timing_Hour })
                .Index(t => t.Section_ID);
            
            CreateTable(
                "dbo.TeacherSections",
                c => new
                    {
                        Teacher_ID = c.Int(nullable: false),
                        Section_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Teacher_ID, t.Section_ID })
                .ForeignKey("dbo.Teachers", t => t.Teacher_ID, cascadeDelete: true)
                .ForeignKey("dbo.Sections", t => t.Section_ID, cascadeDelete: true)
                .Index(t => t.Teacher_ID)
                .Index(t => t.Section_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SelectedEntries", "TutorialSection_ID", "dbo.Sections");
            DropForeignKey("dbo.SelectedEntries", "PracticalSection_ID", "dbo.Sections");
            DropForeignKey("dbo.SelectedEntries", "MainSection_ID", "dbo.Sections");
            DropForeignKey("dbo.SelectedEntries", "Course_COMCOD", "dbo.Courses");
            DropForeignKey("dbo.TeacherSections", "Section_ID", "dbo.Sections");
            DropForeignKey("dbo.TeacherSections", "Teacher_ID", "dbo.Teachers");
            DropForeignKey("dbo.Sections", "Course_COMCOD", "dbo.Courses");
            DropForeignKey("dbo.TimingSections", "Section_ID", "dbo.Sections");
            DropForeignKey("dbo.TimingSections", new[] { "Timing_Day", "Timing_Hour" }, "dbo.Timings");
            DropIndex("dbo.TeacherSections", new[] { "Section_ID" });
            DropIndex("dbo.TeacherSections", new[] { "Teacher_ID" });
            DropIndex("dbo.TimingSections", new[] { "Section_ID" });
            DropIndex("dbo.TimingSections", new[] { "Timing_Day", "Timing_Hour" });
            DropIndex("dbo.SelectedEntries", new[] { "TutorialSection_ID" });
            DropIndex("dbo.SelectedEntries", new[] { "PracticalSection_ID" });
            DropIndex("dbo.SelectedEntries", new[] { "MainSection_ID" });
            DropIndex("dbo.SelectedEntries", new[] { "Course_COMCOD" });
            DropIndex("dbo.Sections", new[] { "Course_COMCOD" });
            DropTable("dbo.TeacherSections");
            DropTable("dbo.TimingSections");
            DropTable("dbo.SelectedEntries");
            DropTable("dbo.Teachers");
            DropTable("dbo.Timings");
            DropTable("dbo.Sections");
            DropTable("dbo.Courses");
        }
    }
}
