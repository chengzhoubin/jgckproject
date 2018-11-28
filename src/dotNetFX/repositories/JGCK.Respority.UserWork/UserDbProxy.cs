using JGCK.Framework.EF;

namespace JGCK.Respority.UserWork
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UserDbProxy : AbstractUnitOfWork
    {
        public UserDbProxy()
            : base("name=UserDbProxy")
        {
        }

        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Role> Role { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .Property(e => e.Pwd)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.ContactNo)
                .IsUnicode(false);


            modelBuilder.Entity<PersonDoctor>()
                .HasRequired(p => p.WithPerson)
                .WithOptional(pd => pd.Doctor)
                .Map(m => m.MapKey("PersonId"))
                .WillCascadeOnDelete(true);
        }
    }
}
