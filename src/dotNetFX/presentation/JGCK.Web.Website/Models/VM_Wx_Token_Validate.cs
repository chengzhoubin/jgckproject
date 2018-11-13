using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSMY_WxWeb.Models
{
    public class VM_Wx_Token_Validate
    {
        public Guid TokenId { get; set; }

        public string UserToken { get; set; }

        public long? UserId { get; set; }
    }
}