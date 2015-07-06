namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NonRequiredDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Addresses", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.Tenants", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Tenants", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.Lookups", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Lookups", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.LookupTypes", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.LookupTypes", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.Organisations", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Organisations", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.Contacts", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Contacts", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.RolePermissions", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.RolePermissions", "UpdatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RolePermissions", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RolePermissions", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Contacts", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Contacts", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Organisations", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Organisations", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LookupTypes", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LookupTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Lookups", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Lookups", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tenants", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tenants", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Addresses", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Addresses", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
