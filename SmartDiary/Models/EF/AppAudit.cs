using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDiary.Models.EF
{
    public interface IAudit
    {
        void Audit(EntityState state, DateTime auditedOn);
    }

    public abstract class AppAudit : IAudit
    {
        protected AppAudit() { Id = Guid.NewGuid(); }

        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "datetime2"), ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }
       
        [Column(TypeName = "datetime2"), ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }
        [Column, MaxLength(50), ScaffoldColumn(false), Timestamp]
        protected virtual byte[] RowVersion { get; set; }
        [Column, ScaffoldColumn(false)]
        protected int Revision { get; set; }

        void IAudit.Audit(EntityState state, DateTime auditedOn)
        {
            ModifiedOn = auditedOn;

            if (state == EntityState.Added)
            {
                CreatedOn = auditedOn;
            }
        }
    }
}
