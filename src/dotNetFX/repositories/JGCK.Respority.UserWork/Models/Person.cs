namespace JGCK.Respority.UserWork
{
    using JGCK.Framework.Repository;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Person")]
    public partial class Person : AbstractDomainEntity
    {
        [Required] [StringLength(150)] public string Name { get; set; }

        [StringLength(300)] public string HeadPicture { get; set; }

        [StringLength(50)] public string StaffNO { get; set; }

        public bool Sex { get; set; }

        [StringLength(100)] public string Position { get; set; }

        [StringLength(50)] public string Account { get; set; }

        [StringLength(50)] public string Pwd { get; set; }

        [StringLength(100)] public string Email { get; set; }

        [StringLength(50)] public string ContactNO { get; set; }

        public bool PersonType { get; set; }

        public bool IsActive { get; set; }

        public long? DepartmentId { get; set; }

        public long? RoleId { get; set; }

        [ForeignKey("RoleId")] public virtual Role Role { get; set; }
    }
}
