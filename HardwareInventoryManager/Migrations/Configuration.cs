namespace HardwareInventoryManager.Migrations
{
    using HardwareInventoryManager.Services;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HardwareInventoryManager.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "HardwareInventoryManager.Models.ApplicationDbContext";
        }

        protected override void Seed(HardwareInventoryManager.Models.ApplicationDbContext context)
        {
            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();

            //  This method will be called after migrating to the latest version.
            CustomApplicationDbContext customContext = new CustomApplicationDbContext();
            SeedService initialSeed = new SeedService(customContext);
        }
    }
}
