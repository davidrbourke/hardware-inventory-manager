namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactConstraints : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Contacts", "EmailAddress", c => c.String(nullable: false));
            AlterColumn("dbo.Contacts", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "Description", c => c.String());
            AlterColumn("dbo.Contacts", "EmailAddress", c => c.String());
            AlterColumn("dbo.Contacts", "Name", c => c.String());
        }
    }
}
