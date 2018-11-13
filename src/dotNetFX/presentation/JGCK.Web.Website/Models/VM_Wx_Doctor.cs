using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSMY_WxWeb.Models
{
    /// <summary>
    /// 微信端医生数据
    /// </summary>
    public class VM_Wx_Doctor
    {
        public string name { get; set; }

        public string desc { get; set; }

        public string iconSrc { get; set; }

        public string url { get; set; }

        public string jobtitle { get; set; }

        public string jobtitle_ext { get; set; }

        public string firstWorkHospital { get; set; }

        public string firstWorkHospitalshort { get; set; }
    }
}