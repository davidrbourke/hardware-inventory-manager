namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class settingtype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSettings", "SettingType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSettings", "SettingType");
        }
    }
}
