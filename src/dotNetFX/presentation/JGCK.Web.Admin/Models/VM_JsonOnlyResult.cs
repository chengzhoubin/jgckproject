using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSMY_AdminWeb.Models
{
    public class VM_JsonOnlyResult
    {
        public bool Result { get; set; }
        public string Err { get; set; }
        public object Value { get; set; }
    }
}