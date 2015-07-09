namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mtom : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUserTenants",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Tenant_TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Tenant_TenantId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tenants", t => t.Tenant_TenantId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Tenant_TenantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserTenants", "Tenant_TenantId", "dbo.Tenants");
            DropForeignKey("dbo.ApplicationUserTenants", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserTenants", new[] { "Tenant_TenantId" });
            DropIndex("dbo.ApplicationUserTenants", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserTenants");
        }
    }
}
