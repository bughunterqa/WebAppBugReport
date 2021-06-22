namespace WebAppBugReport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class summarystringlength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bugs", "Summary", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bugs", "Summary", c => c.String());
        }
    }
}
