using System.ComponentModel;
using JGCK.Respority.UserWork;

namespace JGCK.Respority.UserWork
{
    using JGCK.Framework.Repository;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Person")]
    public partial class Person : AbstractDomainEntity, IEntity<UserDbProxy>
    {
        [Required] [StringLength(50)] public string Name { get; set; }

        [Required] [StringLength(20)] public string RealName { get; set; }

        [StringLength(300)] public string HeadPicture { get; set; }

        [StringLength(50)] public string StaffNo { get; set; }

        public Gender Sex { get; set; }

        [Required] [StringLength(30)] public string IdCard { get; set; }

        [StringLength(100)] public string Position { get; set; }

        [StringLength(50)] public string Account { get; set; }

        [StringLength(50)] public string Pwd { get; set; }

        [StringLength(100)] public string Email { get; set; }

        [StringLength(50)] public string ContactNo { get; set; }

        public OnJobType PersonType { get; set; }

        public bool IsActive { get; set; } = false;

        public bool IsDoctor { get; set; } = false;

        public long? DepartmentId { get; set; }

        [StringLength(150)]
        public string DepartmentName { get; set; }

        public long? RoleId { get; set; }

        [ForeignKey("RoleId")] public virtual Role Role { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? LeaveDate { get; set; }

        public DateTime? PracticeBeginDate { get; set; }

        public DateTime? PracticeEndDate { get; set; }

        public EducationType Education { get; set; }

        [StringLength(50)] public string GraduateInstitution { get; set; }

        [StringLength(50)] public string Major { get; set; }

        [StringLength(30)] public string Phone { get; set; }

        public string FamliyAddress { get; set; }

        [StringLength(20)] public string EmergencyContact { get; set; }

        [StringLength(30)] public string EmergencyPhone { get; set; }

        public string Note { get; set; }

        public bool IsDeleted { get; set; }

        public virtual PersonDoctor Doctor { get; set; }
    }

    public enum Gender
    {
        [Description("��")] Male = 1,
        [Description("Ů")] Female
    }

    public enum OnJobType
    {
        //[Description("��")] Null,
        [Description("��ְ")] OnWork = 1,
        [Description("��ְ")] Fired,
        [Description("ʵϰ����")] OnPractice,
        [Description("ʵϰ����")] OverPractice
    }

    public enum EducationType
    {
        //[Description("��")] Null,

        /// <summary>
        /// ����
        /// </summary>
        [Description("����")] Junior = 1,

        /// <summary>
        /// ��ר
        /// </summary>
        [Description("��ר")] TechnicalSecondary,

        /// <summary>
        /// ��ְ
        /// </summary>
        [Description("��ְ")] HigherVocational,

        /// <summary>
        /// ����
        /// </summary>
        [Description("����")] HighSchool,

        /// <summary>
        /// ר��
        /// </summary>
        [Description("ר��")] JuniorCollege,

        /// <summary>
        /// ����
        /// </summary>
        [Description("����")] RegularCollege,

        /// <summary>
        /// ˶ʿ
        /// </summary>
        [Description("˶ʿ")] Master,

        /// <summary>
        /// ��ʿ
        /// </summary>
        [Description("��ʿ")] Doctor,

        /// <summary>
        /// ��ʿ��
        /// </summary>
        [Description("��ʿ��")] PostDoctor
    }
}
