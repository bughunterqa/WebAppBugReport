namespace WebAppBugReport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bugs", "Updated", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bugs", "Updated");
        }
    }
}
