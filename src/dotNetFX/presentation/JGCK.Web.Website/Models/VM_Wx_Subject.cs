using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSMY_WxWeb.Models
{
    public class VM_Wx_Subject
    {
        public long subjectId { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public List<VM_Wx_SubjectImage> Images { get; set; }

        public List<VM_Wx_ClassInfo> classes { get; set; }
    }

    public class VM_Wx_SubjectImage
    {
        public string imageurl { get; set; }

        public string url { get; set; }
    }
}