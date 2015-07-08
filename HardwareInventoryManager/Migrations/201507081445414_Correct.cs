namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Correct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DashboardUpdates",
                c => new
                    {
                        DashboadUpdatesId = c.Int(nullable: false, identity: true),
                        MessageDate = c.DateTime(nullable: false),
                        Message = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DashboadUpdatesId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DashboardUpdates");
        }
    }
}
