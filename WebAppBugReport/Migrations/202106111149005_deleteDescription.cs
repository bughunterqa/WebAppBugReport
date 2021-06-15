namespace WebAppBugReport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteDescription : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bugs", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bugs", "Description", c => c.String());
        }
    }
}
