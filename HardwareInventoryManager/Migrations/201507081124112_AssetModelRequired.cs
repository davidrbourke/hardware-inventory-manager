namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetModelRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assets", "Model", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assets", "Model", c => c.String());
        }
    }
}
