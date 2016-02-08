namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class c1 : DbMigration
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
                "dbo.ApplicationSettings",
                c => new
                    {
                        ApplicationSettingId = c.Int(nullable: false, identity: true),
                        SettingId = c.Int(nullable: false),
                        Value = c.String(),
                        ScopeType = c.Int(nullable: false),
                        TenantId = c.Int(nullable: false),
                        UserId = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationSettingId)
                .ForeignKey("dbo.Settings", t => t.SettingId)
                .Index(t => t.SettingId);
            
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
            
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        AssetId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        AssetMakeId = c.Int(nullable: false),
                        Model = c.String(nullable: false),
                        SerialNumber = c.String(),
                        PurchaseDate = c.DateTime(),
                        WarrantyPeriodId = c.Int(nullable: false),
                        ObsolescenseDate = c.DateTime(),
                        PricePaid = c.Decimal(precision: 18, scale: 2),
                        CategoryId = c.Int(nullable: false),
                        LocationDescription = c.String(),
                        AssetDetailId = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AssetId)
                .ForeignKey("dbo.Lookups", t => t.AssetMakeId)
                .ForeignKey("dbo.Lookups", t => t.CategoryId)
                .ForeignKey("dbo.AssetDetails", t => t.AssetDetailId)
                .ForeignKey("dbo.Tenants", t => t.TenantId)
                .ForeignKey("dbo.Lookups", t => t.WarrantyPeriodId)
                .Index(t => t.TenantId)
                .Index(t => t.AssetMakeId)
                .Index(t => t.WarrantyPeriodId)
                .Index(t => t.CategoryId)
                .Index(t => t.AssetDetailId);
            
            CreateTable(
                "dbo.Lookups",
                c => new
                    {
                        LookupId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        LookupTypeId = c.Int(nullable: false),
                        AssociatedNumericValue = c.Int(nullable: false),
                        AssociatedText = c.String(),
                        TenantId = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LookupId)
                .ForeignKey("dbo.Tenants", t => t.TenantId)
                .ForeignKey("dbo.LookupTypes", t => t.LookupTypeId)
                .Index(t => t.LookupTypeId)
                .Index(t => t.TenantId);
            
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        TenantId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                        OrganisationAddress_AddressId = c.Int(),
                    })
                .PrimaryKey(t => t.TenantId)
                .ForeignKey("dbo.Addresses", t => t.OrganisationAddress_AddressId)
                .Index(t => t.OrganisationAddress_AddressId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ForcePasswordReset = c.Boolean(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        OrganisationalRole = c.String(),
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
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                "dbo.AssetDetails",
                c => new
                    {
                        AssetDetailId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        ComputerName = c.String(),
                        WMIStatus = c.String(),
                        SSHStatus = c.String(),
                        CurrentOperatingSystem = c.String(),
                        ServicePackLevel = c.String(),
                        ActiveNetworkAdapter = c.String(),
                        IPAddress = c.String(),
                        MACAddress = c.String(),
                        DNSServer = c.String(),
                        SubnetMask = c.String(),
                        WINSServer = c.String(),
                        RegisteredUserName = c.String(),
                        DomainWorkgroup = c.String(),
                        DaysSinceDomainAccountUpdate = c.String(),
                        NumberOfProcessors = c.String(),
                        NumberOfCores = c.String(),
                        LogicalProcessorCount = c.String(),
                        CPU = c.String(),
                        SystemMemoryMB = c.String(),
                        VideoCard = c.String(),
                        VideoCardMemoryMB = c.String(),
                        SoundCard = c.String(),
                        DiskDrive = c.String(),
                        DiskDriveSizeGB = c.String(),
                        OpticalDrive = c.String(),
                        BIOS = c.String(),
                        BIOSSerialNumber = c.String(),
                        BIOSManufacturer = c.String(),
                        BIOSReleaseDate = c.String(),
                        MachineType = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AssetDetailId);
            
            CreateTable(
                "dbo.BulkImports",
                c => new
                    {
                        BulkImportId = c.Int(nullable: false, identity: true),
                        ImportText = c.String(),
                        TenantId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BulkImportId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        MessageRead = c.Boolean(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContactId);
            
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
                "dbo.Emails",
                c => new
                    {
                        EmailId = c.Int(nullable: false, identity: true),
                        SenderEmailAddress = c.String(),
                        RecipientsEmailAddress = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        TenantId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmailId);
            
            CreateTable(
                "dbo.QuoteRequests",
                c => new
                    {
                        QuoteRequestId = c.Int(nullable: false, identity: true),
                        DateRequired = c.DateTime(),
                        Quantity = c.Int(),
                        SpecificationDetails = c.String(),
                        QuoteResponseId = c.Int(),
                        TenantId = c.Int(nullable: false),
                        CategoryId = c.Int(),
                        QuoteRequestStatusId = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.QuoteRequestId)
                .ForeignKey("dbo.Lookups", t => t.CategoryId)
                .ForeignKey("dbo.Lookups", t => t.QuoteRequestStatusId)
                .Index(t => t.CategoryId)
                .Index(t => t.QuoteRequestStatusId);
            
            CreateTable(
                "dbo.QuoteResponses",
                c => new
                    {
                        QuoteReposonseId = c.Int(nullable: false),
                        QuoteCostPerItem = c.Decimal(precision: 18, scale: 2),
                        QuoteCostTotal = c.Decimal(precision: 18, scale: 2),
                        Notes = c.String(),
                        QuoteRequestId = c.Int(nullable: false),
                        TenantId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.QuoteReposonseId)
                .ForeignKey("dbo.QuoteRequests", t => t.QuoteReposonseId)
                .Index(t => t.QuoteReposonseId);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        RolePersmissionId = c.Int(nullable: false, identity: true),
                        Controller = c.String(),
                        Action = c.String(),
                        Role = c.String(),
                        IsAllowed = c.Boolean(nullable: false),
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
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.QuoteResponses", "QuoteReposonseId", "dbo.QuoteRequests");
            DropForeignKey("dbo.QuoteRequests", "QuoteRequestStatusId", "dbo.Lookups");
            DropForeignKey("dbo.QuoteRequests", "CategoryId", "dbo.Lookups");
            DropForeignKey("dbo.Assets", "WarrantyPeriodId", "dbo.Lookups");
            DropForeignKey("dbo.Assets", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.Assets", "AssetDetailId", "dbo.AssetDetails");
            DropForeignKey("dbo.Assets", "CategoryId", "dbo.Lookups");
            DropForeignKey("dbo.Assets", "AssetMakeId", "dbo.Lookups");
            DropForeignKey("dbo.Lookups", "LookupTypeId", "dbo.LookupTypes");
            DropForeignKey("dbo.Lookups", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.ApplicationUserTenants", "Tenant_TenantId", "dbo.Tenants");
            DropForeignKey("dbo.ApplicationUserTenants", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tenants", "OrganisationAddress_AddressId", "dbo.Addresses");
            DropForeignKey("dbo.ApplicationSettings", "SettingId", "dbo.Settings");
            DropIndex("dbo.ApplicationUserTenants", new[] { "Tenant_TenantId" });
            DropIndex("dbo.ApplicationUserTenants", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.QuoteResponses", new[] { "QuoteReposonseId" });
            DropIndex("dbo.QuoteRequests", new[] { "QuoteRequestStatusId" });
            DropIndex("dbo.QuoteRequests", new[] { "CategoryId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Tenants", new[] { "OrganisationAddress_AddressId" });
            DropIndex("dbo.Lookups", new[] { "TenantId" });
            DropIndex("dbo.Lookups", new[] { "LookupTypeId" });
            DropIndex("dbo.Assets", new[] { "AssetDetailId" });
            DropIndex("dbo.Assets", new[] { "CategoryId" });
            DropIndex("dbo.Assets", new[] { "WarrantyPeriodId" });
            DropIndex("dbo.Assets", new[] { "AssetMakeId" });
            DropIndex("dbo.Assets", new[] { "TenantId" });
            DropIndex("dbo.ApplicationSettings", new[] { "SettingId" });
            DropTable("dbo.ApplicationUserTenants");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.QuoteResponses");
            DropTable("dbo.QuoteRequests");
            DropTable("dbo.Emails");
            DropTable("dbo.DashboardUpdates");
            DropTable("dbo.Contacts");
            DropTable("dbo.BulkImports");
            DropTable("dbo.AssetDetails");
            DropTable("dbo.LookupTypes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Tenants");
            DropTable("dbo.Lookups");
            DropTable("dbo.Assets");
            DropTable("dbo.Settings");
            DropTable("dbo.ApplicationSettings");
            DropTable("dbo.Addresses");
        }
    }
}
