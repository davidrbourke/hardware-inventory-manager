namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
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
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AddressId);
            
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        AssetId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        AssetMakeId = c.Int(nullable: false),
                        Model = c.String(),
                        SerialNumber = c.String(),
                        PurchaseDate = c.DateTime(),
                        WarrantyPeriodId = c.Int(nullable: false),
                        ObsolescenseDate = c.DateTime(),
                        PricePaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CategoryId = c.Int(nullable: false),
                        LocationDescription = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AssetId)
                .ForeignKey("dbo.Lookups", t => t.AssetMakeId)
                .ForeignKey("dbo.Lookups", t => t.CategoryId)
                .ForeignKey("dbo.Tenants", t => t.TenantId)
                .ForeignKey("dbo.Lookups", t => t.WarrantyPeriodId)
                .Index(t => t.TenantId)
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
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
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
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LookupTypeId);
            
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        TenantId = c.Int(nullable: false, identity: true),
                        TenantOrganisationId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TenantId)
                .ForeignKey("dbo.Organisations", t => t.TenantOrganisationId)
                .Index(t => t.TenantOrganisationId);
            
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
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContactId);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        RolePersmissionId = c.Int(nullable: false, identity: true),
                        Controller = c.String(),
                        Action = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RolePersmissionId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Assets", "WarrantyPeriodId", "dbo.Lookups");
            DropForeignKey("dbo.Assets", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.Tenants", "TenantOrganisationId", "dbo.Organisations");
            DropForeignKey("dbo.Organisations", "OrganisationAddress_AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Assets", "CategoryId", "dbo.Lookups");
            DropForeignKey("dbo.Assets", "AssetMakeId", "dbo.Lookups");
            DropForeignKey("dbo.Lookups", "LookupTypeId", "dbo.LookupTypes");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Organisations", new[] { "OrganisationAddress_AddressId" });
            DropIndex("dbo.Tenants", new[] { "TenantOrganisationId" });
            DropIndex("dbo.Lookups", new[] { "LookupTypeId" });
            DropIndex("dbo.Assets", new[] { "CategoryId" });
            DropIndex("dbo.Assets", new[] { "WarrantyPeriodId" });
            DropIndex("dbo.Assets", new[] { "AssetMakeId" });
            DropIndex("dbo.Assets", new[] { "TenantId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.Contacts");
            DropTable("dbo.Organisations");
            DropTable("dbo.Tenants");
            DropTable("dbo.LookupTypes");
            DropTable("dbo.Lookups");
            DropTable("dbo.Assets");
            DropTable("dbo.Addresses");
        }
    }
}
