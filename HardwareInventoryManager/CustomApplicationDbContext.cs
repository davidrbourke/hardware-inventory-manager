using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager
{
    public class CustomApplicationDbContext : ApplicationDbContext
    {
        public CustomApplicationDbContext()
            : base()
        {
        }

        public override int SaveChanges()
        {
            SetCreatedModifiedDates();
            return base.SaveChanges();
        }

        private void SetCreatedModifiedDates()
        {
            var entries = this.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified || t.State == EntityState.Added);
            var currentTime = DateTime.Now;

            foreach (var entry in entries)
            {
                var entityBase = entry.Entity as ModelEntity;

                if (entityBase != null)
                {
                    if (entry.State == EntityState.Added && (entityBase.CreatedDate == DateTime.MinValue || entityBase.CreatedDate == null))
                    {
                        entityBase.CreatedDate = currentTime;
                    }
                    entityBase.UpdatedDate = currentTime;
                }
            }
        }
    }
}