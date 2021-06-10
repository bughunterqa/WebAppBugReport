namespace WebAppBugReport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bugs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Summary = c.String(),
                        Description = c.String(),
                        ExpectedResult = c.String(),
                        ActualResult = c.String(),
                        StepToReproduce = c.String(),
                        BugImg = c.String(),
                        Comment = c.String(),
                        Link = c.String(),
                        Date = c.DateTime(nullable: false),
                        PriorityId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        ResultId = c.Int(),
                        UserId = c.Int(),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Priorities", t => t.PriorityId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Results", t => t.ResultId)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.PriorityId)
                .Index(t => t.StatusId)
                .Index(t => t.ResultId)
                .Index(t => t.UserId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Priorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Project_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bugs", "UserId", "dbo.Users");
            DropForeignKey("dbo.Bugs", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Bugs", "ResultId", "dbo.Results");
            DropForeignKey("dbo.Bugs", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.UserProjects", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Bugs", "PriorityId", "dbo.Priorities");
            DropIndex("dbo.UserProjects", new[] { "Project_Id" });
            DropIndex("dbo.UserProjects", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Bugs", new[] { "ProjectId" });
            DropIndex("dbo.Bugs", new[] { "UserId" });
            DropIndex("dbo.Bugs", new[] { "ResultId" });
            DropIndex("dbo.Bugs", new[] { "StatusId" });
            DropIndex("dbo.Bugs", new[] { "PriorityId" });
            DropTable("dbo.UserProjects");
            DropTable("dbo.Status");
            DropTable("dbo.Results");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.Priorities");
            DropTable("dbo.Bugs");
        }
    }
}
