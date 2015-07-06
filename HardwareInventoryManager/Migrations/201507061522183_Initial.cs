namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        AddressLine3 = c.String(),
                        AddressLine4 = c.String(),
                        Postcode = c.String(),
                        TelephoneNumber = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId);
            
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        TenantId = c.Int(nullable: false, identity: true),
                        TenantOrganisationId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        AssetId = c.Int(),
                        AssetMakeId = c.Int(),
                        Model = c.String(),
                        SerialNumber = c.String(),
                        PurchaseDate = c.DateTime(),
                        WarrantyPeriodId = c.Int(),
                        ObsolescenseDate = c.DateTime(),
                        PricePaid = c.Decimal(precision: 18, scale: 2),
                        CategoryId = c.Int(),
                        LocationDescription = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.TenantId)
                .ForeignKey("dbo.Lookups", t => t.AssetMakeId)
                .ForeignKey("dbo.Lookups", t => t.CategoryId)
                .ForeignKey("dbo.Lookups", t => t.WarrantyPeriodId)
                .ForeignKey("dbo.Organisations", t => t.TenantOrganisationId)
                .Index(t => t.TenantOrganisationId)
                .Index(t => t.AssetMakeId)
                .Index(t => t.WarrantyPeriodId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Lookups",
                c => new
                    {
                        LookupId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        LookupTypeId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LookupId)
                .ForeignKey("dbo.LookupTypes", t => t.LookupTypeId)
                .Index(t => t.LookupTypeId);
            
            CreateTable(
                "dbo.LookupTypes",
                c => new
                    {
                        LookupTypeId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LookupTypeId);
            
            CreateTable(
                "dbo.Organisations",
                c => new
                    {
                        OrganisationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        OrganisationAddress_AddressId = c.Int(),
                    })
                .PrimaryKey(t => t.OrganisationId)
                .ForeignKey("dbo.Addresses", t => t.OrganisationAddress_AddressId)
                .Index(t => t.OrganisationAddress_AddressId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EmailAddress = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ContactId);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        RolePersmissionId = c.Int(nullable: false, identity: true),
                        Controller = c.String(),
                        Action = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RolePersmissionId);
            
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Tenants", "TenantOrganisationId", "dbo.Organisations");
            DropForeignKey("dbo.Tenants", "WarrantyPeriodId", "dbo.Lookups");
            DropForeignKey("dbo.Organisations", "OrganisationAddress_AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Tenants", "CategoryId", "dbo.Lookups");
            DropForeignKey("dbo.Tenants", "AssetMakeId", "dbo.Lookups");
            DropForeignKey("dbo.Lookups", "LookupTypeId", "dbo.LookupTypes");
            DropIndex("dbo.Organisations", new[] { "OrganisationAddress_AddressId" });
            DropIndex("dbo.Lookups", new[] { "LookupTypeId" });
            DropIndex("dbo.Tenants", new[] { "CategoryId" });
            DropIndex("dbo.Tenants", new[] { "WarrantyPeriodId" });
            DropIndex("dbo.Tenants", new[] { "AssetMakeId" });
            DropIndex("dbo.Tenants", new[] { "TenantOrganisationId" });
            DropTable("dbo.RolePermissions");
            DropTable("dbo.Contacts");
            DropTable("dbo.Organisations");
            DropTable("dbo.LookupTypes");
            DropTable("dbo.Lookups");
            DropTable("dbo.Tenants");
            DropTable("dbo.Addresses");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
        }
    }
}
