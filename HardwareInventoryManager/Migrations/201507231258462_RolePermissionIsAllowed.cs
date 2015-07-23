namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RolePermissionIsAllowed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RolePermissions", "IsAllowed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RolePermissions", "IsAllowed");
        }
    }
}
