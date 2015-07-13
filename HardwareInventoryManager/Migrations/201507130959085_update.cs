namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Organisations", "OrganisationAddress_AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Tenants", "TenantOrganisationId", "dbo.Organisations");
            DropForeignKey("dbo.AspNetUsers", "TenantId", "dbo.Tenants");
            DropIndex("dbo.Tenants", new[] { "TenantOrganisationId" });
            DropIndex("dbo.Organisations", new[] { "OrganisationAddress_AddressId" });
            DropIndex("dbo.AspNetUsers", new[] { "TenantId" });
            CreateTable(
                "dbo.DashboardUpdates",
                c => new
                    {
                        DashboadUpdatesId = c.Int(nullable: false, identity: true),
                        MessageDate = c.DateTime(nullable: false),
                        Message = c.String(),
                        Complete = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DashboadUpdatesId);
            
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
            
            AddColumn("dbo.Lookups", "TenantId", c => c.Int());
            AddColumn("dbo.Tenants", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Tenants", "OrganisationAddress_AddressId", c => c.Int());
            CreateIndex("dbo.Lookups", "TenantId");
            CreateIndex("dbo.Tenants", "OrganisationAddress_AddressId");
            AddForeignKey("dbo.Tenants", "OrganisationAddress_AddressId", "dbo.Addresses", "AddressId");
            AddForeignKey("dbo.Lookups", "TenantId", "dbo.Tenants", "TenantId");
            DropColumn("dbo.Tenants", "TenantOrganisationId");
            DropColumn("dbo.AspNetUsers", "TenantId");
            DropTable("dbo.Organisations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Organisations",
                c => new
                    {
                        OrganisationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                        OrganisationAddress_AddressId = c.Int(),
                    })
                .PrimaryKey(t => t.OrganisationId);
            
            AddColumn("dbo.AspNetUsers", "TenantId", c => c.Int(nullable: false));
            AddColumn("dbo.Tenants", "TenantOrganisationId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Lookups", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.ApplicationUserTenants", "Tenant_TenantId", "dbo.Tenants");
            DropForeignKey("dbo.ApplicationUserTenants", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tenants", "OrganisationAddress_AddressId", "dbo.Addresses");
            DropIndex("dbo.ApplicationUserTenants", new[] { "Tenant_TenantId" });
            DropIndex("dbo.ApplicationUserTenants", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Tenants", new[] { "OrganisationAddress_AddressId" });
            DropIndex("dbo.Lookups", new[] { "TenantId" });
            DropColumn("dbo.Tenants", "OrganisationAddress_AddressId");
            DropColumn("dbo.Tenants", "Name");
            DropColumn("dbo.Lookups", "TenantId");
            DropTable("dbo.ApplicationUserTenants");
            DropTable("dbo.DashboardUpdates");
            CreateIndex("dbo.AspNetUsers", "TenantId");
            CreateIndex("dbo.Organisations", "OrganisationAddress_AddressId");
            CreateIndex("dbo.Tenants", "TenantOrganisationId");
            AddForeignKey("dbo.AspNetUsers", "TenantId", "dbo.Tenants", "TenantId");
            AddForeignKey("dbo.Tenants", "TenantOrganisationId", "dbo.Organisations", "OrganisationId");
            AddForeignKey("dbo.Organisations", "OrganisationAddress_AddressId", "dbo.Addresses", "AddressId");
        }
    }
}
