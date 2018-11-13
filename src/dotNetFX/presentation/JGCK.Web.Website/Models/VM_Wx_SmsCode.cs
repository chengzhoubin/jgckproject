using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSMY_WxWeb.Models
{
    public class VM_Wx_SmsCode
    {
        public string Phone { get; set; }

        public string UnionID { get; set; }

        public long? UserID { get; set; }
    }
}