namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleToPermissions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RolePermissions", "Role", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RolePermissions", "Role");
        }
    }
}
