namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForcePasswordResetr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ForcePasswordReset", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ForcePasswordReset");
        }
    }
}
