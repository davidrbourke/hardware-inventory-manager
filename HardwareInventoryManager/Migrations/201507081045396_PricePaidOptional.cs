namespace HardwareInventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PricePaidOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assets", "PricePaid", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assets", "PricePaid", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
