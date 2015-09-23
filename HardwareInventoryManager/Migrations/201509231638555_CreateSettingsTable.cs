namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Settings_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        SettingId = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        DataType = c.Int(nullable: false),
                        SettingType = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SettingId);
            
            AddColumn("dbo.ApplicationSettings", "SettingId", c => c.Int(nullable: false));
            CreateIndex("dbo.ApplicationSettings", "SettingId");
            AddForeignKey("dbo.ApplicationSettings", "SettingId", "dbo.Settings", "SettingId");
            DropColumn("dbo.ApplicationSettings", "Key");
            DropColumn("dbo.ApplicationSettings", "DataType");
            DropColumn("dbo.ApplicationSettings", "SettingType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationSettings", "SettingType", c => c.Int(nullable: false));
            AddColumn("dbo.ApplicationSettings", "DataType", c => c.Int(nullable: false));
            AddColumn("dbo.ApplicationSettings", "Key", c => c.String());
            DropForeignKey("dbo.ApplicationSettings", "SettingId", "dbo.Settings");
            DropIndex("dbo.ApplicationSettings", new[] { "SettingId" });
            DropColumn("dbo.ApplicationSettings", "SettingId");
            DropTable("dbo.Settings");
        }
    }
}
