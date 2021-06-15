namespace WebAppBugReport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactoringBugsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bugs", "ResultId", "dbo.Results");
            DropIndex("dbo.Bugs", new[] { "ResultId" });
            DropColumn("dbo.Bugs", "Comment");
            DropColumn("dbo.Bugs", "ResultId");
            DropTable("dbo.Results");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Bugs", "ResultId", c => c.Int());
            AddColumn("dbo.Bugs", "Comment", c => c.String());
            CreateIndex("dbo.Bugs", "ResultId");
            AddForeignKey("dbo.Bugs", "ResultId", "dbo.Results", "Id");
        }
    }
}
