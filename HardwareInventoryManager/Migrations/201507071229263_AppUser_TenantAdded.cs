namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppUser_TenantAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TenantId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "TenantId");
            AddForeignKey("dbo.AspNetUsers", "TenantId", "dbo.Tenants", "TenantId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "TenantId", "dbo.Tenants");
            DropIndex("dbo.AspNetUsers", new[] { "TenantId" });
            DropColumn("dbo.AspNetUsers", "TenantId");
        }
    }
}
