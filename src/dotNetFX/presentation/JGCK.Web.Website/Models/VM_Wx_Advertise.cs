using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSMY_WxWeb.Models
{
    public class VM_Wx_Advertise
    {
        public long AdvID { get; set; }

        public string AdvTitle { get; set; }

        public string ImgUrl { get; set; }

        public string RedirectUrl { get; set; }
    }
}