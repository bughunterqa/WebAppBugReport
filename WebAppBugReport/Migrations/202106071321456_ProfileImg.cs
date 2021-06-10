namespace WebAppBugReport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileImg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ProfileImg", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ProfileImg");
        }
    }
}
