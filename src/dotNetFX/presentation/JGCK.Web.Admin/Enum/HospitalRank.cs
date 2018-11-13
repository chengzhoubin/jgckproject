using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JGCK.Web.Admin.Enum
{
    public enum HospitalRank
    {
        [Display(Name = "未评级")]
        [Description(@"未评级")]
        No_AUDIT = 1,

        [Display(Name = "一级乙等")]
        [Description(@"一级乙等")]
        Rank_Yjyd,

        [Display(Name = "一级甲等")]
        [Description(@"一级甲等")]
        Rank_Yjjd,

        [Display(Name = "二级乙等")]
        [Description(@"二级乙等")]
        Rand_Erjyd,

        [Display(Name = "二级甲等")]
        [Description(@"二级甲等")]
        Rand_Erjjd,

        [Display(Name = "三级乙等")]
        [Description(@"三级乙等")]
        Rand_Sjyd,

        [Display(Name = "三级甲等")]
        [Description(@"三级甲等")]
        Rand_Sjjd,
    }
}