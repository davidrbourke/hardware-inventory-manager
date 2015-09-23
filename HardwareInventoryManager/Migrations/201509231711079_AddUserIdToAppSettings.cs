namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSettings", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSettings", "UserId");
        }
    }
}
