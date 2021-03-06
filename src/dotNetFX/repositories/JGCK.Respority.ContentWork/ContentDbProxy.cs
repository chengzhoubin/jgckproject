using JGCK.Framework.EF;

namespace JGCK.Respority.ContentWork
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ContentDbProxy : AbstractUnitOfWork
    {
        public ContentDbProxy()
            : base("name=ContentDbProxy")
        {
        }

        public virtual DbSet<PortalColumn> PortalColumn { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
