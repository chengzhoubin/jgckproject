using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JGCK.Util.Helper
{
    public static class RegexHelper
    {
        public static Regex RegexChineseIDCardRule { get; private set; }
        public static Regex RegexEmailRule { get; private set; }
        public static Regex RegexPhoneOrMobileRule { get; private set; }

        static RegexHelper()
        {
            RegexChineseIDCardRule = new Regex(@"^[\d]{15,18}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            RegexEmailRule = new Regex(@"^\w+@\w+(\.\w+)+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            RegexPhoneOrMobileRule = new Regex(@"^[\d|\-]{5,20}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}
