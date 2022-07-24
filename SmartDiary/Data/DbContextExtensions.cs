using Microsoft.EntityFrameworkCore;
using SmartDiary.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDiary.Data
{
    public static class DbContextExtensions
    {
        public static void Audit(this DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.Entity is IAudit && (e.State == EntityState.Added || e.State == EntityState.Modified));
            var transDate = DateTime.Now;

            foreach (var entry in entries)
            {
                if (entry.Entity is IAudit auditEntry)
                    auditEntry.Audit(entry.State, transDate);
            }
        }
    }
}
