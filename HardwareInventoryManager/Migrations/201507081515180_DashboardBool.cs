namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DashboardBool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DashboardUpdates", "Complete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DashboardUpdates", "Complete");
        }
    }
}
